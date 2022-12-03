from flask import Flask
from flask import jsonify

import mesa
import numpy as np
from enum import Enum
import math
# Map and agents number for model
# There is switcheroo in coordinates
'''
Python Array
00 01 02
10 11 12
20 21 22

Mesa Grid
02 12 22
01 11 21
00 10 20

We perform left rotation for the array
'''
myMap = [[3, 1, 1, 1, 3, 1, 1, 1, 2],
         [1, 0, 0, 0, 1, 0, 0, 0, 1],
         [1, 0, 0, 0, 1, 0, 0, 0, 1],
         [1, 0, 0, 0, 1, 0, 0, 0, 1],
         [3, 1, 1, 1, 4, 1, 1, 1, 2],
         [1, 0, 0, 0, 1, 0, 0, 0, 1],
         [1, 0, 0, 0, 1, 0, 0, 0, 1],
         [1, 0, 0, 0, 1, 0, 0, 0, 1],
         [3, 1, 1, 1, 2, 1, 1, 1, 2]]

coordsToLocation ={ (0,0): 1, (1,0): 2,(2,0): 3,(3,0): 4,(4,0): 5,(4,1): 6,(4,2): 7,(4,3): 8,(4,4): 9,(4,5): 10,
                   (4,6): 11,(4,7): 12,(4,8): 13,(5,8): 14,(6,8): 15,(7,8): 16,(8,8): 17,(8,7): 18,(8,6): 19,
                   (8,5): 20,(8,4): 21,(7,4): 22,(6,4): 23,(5,4): 24,(3,4): 25,(2,4): 26,(1,4): 27,(0,4): 28,(0,3): 29,
                   (0,2): 30,(0,1): 31}

numberOfAgents = 6
# Formula is square root of number of cars multiplied by two but not less than 2 and not more than 8
#trafficLightTime = min(max(int(math.sqrt(numberOfAgents)*2), 2), 8)

trafficLightTime = 3

Direction = Enum('Direction', ['UP', 'DOWN', 'LEFT', 'RIGHT', 'NONE'])

# Determines where is right based on current direction


def right(direction):
    if direction == Direction.UP:
        return Direction.RIGHT

    if direction == Direction.DOWN:
        return Direction.LEFT

    if direction == Direction.LEFT:
        return Direction.UP

    if direction == Direction.RIGHT:
        return Direction.DOWN

# Determines where is left based on current direction


def left(direction):
    if direction == Direction.UP:
        return Direction.LEFT

    if direction == Direction.DOWN:
        return Direction.RIGHT

    if direction == Direction.LEFT:
        return Direction.DOWN

    if direction == Direction.RIGHT:
        return Direction.UP

# Determines new direction based on random choice made when agent could not encounter any further road ahead.
# Deprecated in our model but for random based choices I left it here


def findDir(currPos, desPos):
    res = np.subtract(currPos, desPos)
    if np.array_equal(res, (0, 1)):
        return Direction.DOWN

    if np.array_equal(res, (0, -1)):
        return Direction.UP

    if np.array_equal(res, (1, 0)):
        return Direction.LEFT

    if np.array_equal(res, (-1, 0)):
        return Direction.RIGHT


# Agent that represents road it has only position it holds in the grid
class RoadAgent(mesa.Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)

    def step(self):
        pass

    def advance(self):
        pass


# Agent that represents left turn in our model, it has only position it holds in the grid
class LeftAgent(mesa.Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)

    def step(self):
        pass

    def advance(self):
        pass


# Agent that represents right turn in our model, it has only position it holds in the grid
class RightAgent(mesa.Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)

    def step(self):
        pass

    def advance(self):
        pass


# Agent that represents intersection in our model, it supports only one ways and straight movement
class FourWayAgent(mesa.Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.greenLeft = False
        self.greenUp = True
        self.counter = 0
    # During step it counts ticks and changes every few ticks and there is no space in between.
    # Also traffic lights go first in simulation so it does not change between step and advance for other agents

    def step(self):
        self.counter += 1
        if self.counter == trafficLightTime:
            if self.greenLeft:
                self.greenLeft = False
            else:
                self.greenLeft = True
            if self.greenUp:
                self.greenUp = False
            else:
                self.greenUp = True
            self.counter = 0

    def advance(self):
        pass


# Represents car in our model. It contains baseline members and direction which it is currently going, desired position
# which is position where it wants to go, next position is the actual position it will take next tick and it can remain
# in the place and waitID which is ID of agent that is blocking the path.
class CarAgent(mesa.Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.direction = Direction.NONE
        self.desiredPos = (0, 0)
        self.nextPos = (0, 0)
        self.waitingID = -1

# Based on current direction it evaluates desired position
    def move(self):
        self.waitingID = -1
        if self.direction == Direction.UP:
            self.desiredPos = (self.pos[0], self.pos[1]+1)
        if self.direction == Direction.DOWN:
            self.desiredPos = (self.pos[0], self.pos[1]-1)

        if self.direction == Direction.LEFT:
            self.desiredPos = (self.pos[0]-1, self.pos[1])

        if self.direction == Direction.RIGHT:
            self.desiredPos = (self.pos[0]+1, self.pos[1])

# When agent encounters traffic lights it needs to behave correctly, either stop or go
    def lights(self, agent):
        if agent.greenUp and (self.direction == Direction.UP or self.direction == Direction.DOWN):
            self.nextPos = self.desiredPos
        elif agent.greenLeft and (self.direction == Direction.LEFT or self.direction == Direction.RIGHT):
            self.nextPos = self.desiredPos
        else:
            self.waitingID = agent.unique_id
            self.nextPos = self.pos

    # Agent can also encounter a traffic jam and here it needs to know what is the first thing blocking it path and what is
    # the last thing. If there is car before it technically is blocking its path but if it is moving it does not.
    # If there is traffic jam then we need to look maybe many cars ahead to find if there is traffic light.
    def stop(self):
        if self.waitingID == -1:
            self.nextPos = self.desiredPos
        # Using sort of linked list we find the traffic light or the first car in the jam.
        # If it is light we determine if we are going next step or if we are staying and for car we go.
        else:
            lastCarDir = self.direction
            lastAgent = model.schedule.agents[self.waitingID]
            while not isinstance(lastAgent, FourWayAgent) and lastAgent.waitingID != -1:
                lastCarDir = lastAgent.direction
                lastAgent = model.schedule.agents[lastAgent.waitingID]

            if isinstance(lastAgent, FourWayAgent):
                if lastAgent.greenUp and (lastCarDir == Direction.UP or lastCarDir == Direction.DOWN):
                    self.nextPos = self.desiredPos
                elif lastAgent.greenLeft and (lastCarDir == Direction.LEFT or lastCarDir == Direction.RIGHT):
                    self.nextPos = self.desiredPos
                else:
                    self.nextPos = self.pos

            else:
                self.nextPos = self.desiredPos

    # Finding another direction which has road connected to current tile.
    # Deprecated in our model
    def noStraight(self):
        neighbors = self.model.gridRoad.get_neighborhood(self.pos, moore=False)
        for i in neighbors:
            cont = self.model.gridRoad.get_cell_list_contents([i])
            if len(cont) == 0:
                continue
            for j in cont:
                if isinstance(j, RoadAgent):
                    self.desiredPos = i
                    self.direction = findDir(self.pos, i)
                    self.nextPos = self.desiredPos

                    break
                elif isinstance(j, FourWayAgent):
                    self.desiredPos = i
                    self.direction = findDir(self.pos, i)
                    self.lights(j)
                    break

    # Step function, gets desired position, checks what is there and based on that it sets next move with methods above
    def step(self):
        self.move()
        if self.model.gridRoad.out_of_bounds(self.desiredPos):
            self.noStraight()

        contents = self.model.gridRoad.get_cell_list_contents(
            [self.desiredPos])
        carIndex = 1
        if len(contents) == 0:
            self.noStraight()
        elif len(contents) > 1:
            if isinstance(contents[0], CarAgent):
                self.waitingID = contents[0].unique_id
                carIndex = 0
            elif isinstance(contents[1], CarAgent):
                self.waitingID = contents[1].unique_id
                carIndex = 1

        if isinstance(contents[1-carIndex], FourWayAgent):
            self.lights(contents[1-carIndex])
        elif isinstance(contents[1-carIndex], LeftAgent):
            self.direction = left(self.direction)
            self.nextPos = self.desiredPos

        elif isinstance(contents[1-carIndex], RightAgent):
            self.direction = right(self.direction)
            self.nextPos = self.desiredPos

        else:
            self.nextPos = self.desiredPos

    # Stop function is called in "advance" so every car had already found what is it waiting for
    def advance(self):
        self.stop()
        self.model.gridRoad.move_agent(self, self.nextPos)


# Initialization of RoadModel, it accepts the map as 2D array and positions and directions of cars as 2D array
# It cointains MultiGrid so multiple agents can be in same cell and Simultaneous activation that prepares the tick
# by first calling step and after that it will call advance.
class RoadModel(mesa.Model):
    def __init__(self, roadMap, numberOfCars):
        self.carCount = 0
        self.gridRoad = mesa.space.MultiGrid(
            len(roadMap), len(roadMap[0]), False)
        self.schedule = mesa.time.SimultaneousActivation(self)
        self.counter = 0
        self.wantedCars = numberOfCars
        for i in range(len(roadMap)):
            for j in range(len(roadMap[i])):
                # Make the traffic light first so it is called in scheduler first and does not change between phases
                # ID is 0
                if roadMap[i][j] == 4:
                    agent = FourWayAgent(self.counter, self)
                    self.schedule.add(agent)
                    self.counter += 1
                    self.gridRoad.place_agent(agent, (i, j))

        # Load map
        for i in range(len(roadMap)):
            for j in range(len(roadMap[i])):
                if roadMap[i][j] == 1:
                    agent = RoadAgent(self.counter, self)
                    self.schedule.add(agent)
                    self.counter += 1
                    self.gridRoad.place_agent(agent, (i, j))
                if roadMap[i][j] == 3:
                    agent = LeftAgent(self.counter, self)
                    self.schedule.add(agent)
                    self.counter += 1
                    self.gridRoad.place_agent(agent, (i, j))
                if roadMap[i][j] == 2:
                    agent = RightAgent(self.counter, self)
                    self.schedule.add(agent)
                    self.counter += 1
                    self.gridRoad.place_agent(agent, (i, j))

    def step(self):
        if self.carCount < self.wantedCars:
            agent = CarAgent(self.counter, self)
            agent.direction = Direction.RIGHT
            self.gridRoad.place_agent(agent, (0, 0))
            self.schedule.add(agent)
            self.counter += 1
            self.carCount += 1

        self.schedule.step()

# --- MAIN ---
model = RoadModel(myMap, numberOfAgents)
# ---

def reset():
    reset_model = RoadModel(myMap, numberOfAgents)
    return reset_model

app = Flask(__name__)

@app.route('/')
def handle_request():

    positions = []
    fourWayAgents = []
    response = {'agents': [],'semaphores': [], 'semaphoreTimer': trafficLightTime}
    n_agent = 1
    n_semaphore = 1
    model.step()

    for i in model.schedule.agents:
        if isinstance(i, CarAgent):
            positions.append(i.nextPos)
            response['agents'].append(
                {'id': n_agent, 'loc': coordsToLocation[i.pos]})
            n_agent += 1
        if isinstance(i, FourWayAgent):
            fourWayAgents.append(i.greenLeft)
            fourWayAgents.append(i.greenUp)
            response['semaphores'].append(
                {'id': n_semaphore, 'greenLeft': i.greenLeft, 'greenUp': i.greenUp})
            n_semaphore += 1

    return jsonify(response)









