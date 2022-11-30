#!/usr/bin/env python
# coding: utf-8

# In[34]:





# In[135]:


import mesa
import numpy as np
from enum import Enum


# In[788]:


# Cleaning agent cleans the dirt agent using wealth variable.
# When dirt agent is cleaned it will change its dirt to 0.
# Agents do not collide.
# Cleaning agents move randomly.
Direction = Enum('Direction', ['UP', 'DOWN', 'LEFT', 'RIGHT','NONE'])
AgentType = Enum('AgentType', ['ROAD', 'CAR', 'TS','TR','TL', 'X'])

def right(direction, pos):
    if direction == Direction.UP:
        return pos.x +1, pos.y+1
    
    if direction == Direction.DOWN:
        return pos.x -1, pos.y-1

    if direction == Direction.LEFT:
        return pos.x -1, pos.y+1

    if direction == Direction.RIGHT:
        return pos.x +1, pos.y-1


def left(direction, pos):
    if direction == Direction.UP:
        return pos.x -1, pos.y+1
    
    if direction == Direction.DOWN:
        return pos.x +1, pos.y-1

    if direction == Direction.LEFT:
        return pos.x -1, pos.y-1

    if direction == Direction.RIGHT:
        return pos.x +1, pos.y+1

def front(direction, pos):
    if direction == Direction.UP:
        return pos.x, pos.y+2
    
    if direction == Direction.DOWN:
        return pos.x, pos.y-2

    if direction == Direction.LEFT:
        return pos.x -2, pos.y

    if direction == Direction.RIGHT:
        return pos.x +2, pos.y
    
class RoadAgent(mesa.Agent):

    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.AgentType = AgentType.ROAD
    def step(self):
        pass
    def advance(self):
        pass
class tJunctionSAgent(mesa.Agent):
    pass

class tJunctionRAgent(mesa.Agent):
    pass

class tJunctionLAgent(mesa.Agent):
    pass

class fourWayAgent(mesa.Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.greenLeft = False
        self.greenUp = True
        self.counter = 0
    def step(self):
        self.counter += 1
    def advance(self):
        print(self.counter)
        if self.counter == 2:
            if self.greenLeft:
                self.greenLeft = False
            else: 
                self.greenLeft = True
            if self.greenUp:
                self.greenUp = False
            else:
                self.greenUp = True
            self.counter = 0
        if self.greenLeft:
            print('LEFT')
        else:
            print('UP')
def findDir(currPos, desPos):
    res = np.subtract(currPos,desPos)
    if np.array_equal(res, (0,1)):
        return Direction.DOWN
    if np.array_equal(res, (0,-1)):
        return Direction.UP
    
    if np.array_equal(res, (1,0)):
        return Direction.LEFT
    
    if np.array_equal(res, (-1,0)):
        return Direction.RIGHT
            
class CarAgent(mesa.Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        self.direction = Direction.NONE
        self.desiredPos = (0,0)
        self.nextPos = (0,0)
    def move(self):
        if self.direction == Direction.UP:
            self.desiredPos = (self.pos[0],self.pos[1]+1)
        if self.direction == Direction.DOWN:
            self.desiredPos = (self.pos[0],self.pos[1]-1)

        if self.direction == Direction.LEFT:
            self.desiredPos = (self.pos[0]-1,self.pos[1])

        if self.direction == Direction.RIGHT:
             self.desiredPos = (self.pos[0]+1,self.pos[1])
    def lights(self,agent):
            if agent.greenUp and (self.direction == Direction.UP or self.direction == Direction.DOWN):
                self.nextPos = self.desiredPos
            elif agent.greenLeft and (self.direction == Direction.LEFT or self.direction == Direction.RIGHT):
                self.nextPos = self.desiredPos
            else:
                self.nextPos = self.pos
    
                
                
    def noStraight(self):
        neighbors = self.model.gridRoad.get_neighborhood(self.pos, moore = False)
        for i in neighbors:
            cont = self.model.gridRoad.get_cell_list_contents([i])
            if len(cont) == 0:
                continue
            for j in cont:
                if isinstance(j,RoadAgent):
                    self.desiredPos = i
                    self.direction = findDir(self.pos,i)
                    self.nextPos = self.desiredPos

                    break
                elif isinstance(j,fourWayAgent):
                    self.desiredPos = i
                    self.direction = findDir(self.pos,i)
                    self.lights(j)
                    break        
                    
    def step(self):
        self.move()
        if self.model.gridRoad.out_of_bounds(self.desiredPos):
            self.noStraight()
            
        contents = self.model.gridRoad.get_cell_list_contents([self.desiredPos])
        if len(contents) > 1:
            self.nextPos = self.pos
        elif len(contents) == 0:
            self.noStraight()

        elif isinstance(contents[0],fourWayAgent):
            self.lights(contents[0])

        else:
            self.nextPos = self.desiredPos
    def advance(self):
        print(self.desiredPos)
        self.model.gridRoad.move_agent(self, self.nextPos)
        


# In[813]:


#myMap =   [[2, 1, 1, 1, 2, 1, 1, 1, 2],
#           [1, 0, 0, 0, 1, 0, 0, 0, 1],
#           [1, 0, 0, 0, 1, 0, 0, 0, 1],
#           [1, 0, 0, 0, 1, 0, 0, 0, 1],
#           [2, 1, 1, 1, 4, 1, 1, 1, 2],
#           [1, 0, 0, 0, 1, 0, 0, 0, 1],
#           [1, 0, 0, 0, 1, 0, 0, 0, 1],
#           [1, 0, 0, 0, 1, 0, 0, 0, 1],
#           [1, 1, 1, 1, 2, 1, 1, 1, 2]]
myMap = [[0,1,0,0,0],
         [0,1,0,0,0],
         [1,4,1,1,1],
         [0,1,0,0,0]]


# In[814]:


class RoadModel(mesa.Model):
    def __init__(self, roadMap):
        #self.num_agents = numberOfCars
        self.gridRoad = mesa.space.MultiGrid(len(roadMap), len(roadMap[0]), False)
        self.schedule = mesa.time.SimultaneousActivation(self)
        self.counter = 0
        
        # Create agents
        for i in range(len(roadMap)):
            for j in range(len(roadMap[i])):
                if roadMap[i][j] == 1:
                    agent = RoadAgent(self.counter, self)
                    self.schedule.add(agent)
                    self.counter +=1
                    self.gridRoad.place_agent(agent,(i,j))
                if roadMap[i][j] == 4:
                    agent = fourWayAgent(self.counter, self)
                    self.schedule.add(agent)
                    self.counter +=1
                    self.gridRoad.place_agent(agent,(i,j))                    
        agent = CarAgent(self.counter, self)
        agent.direction = Direction.LEFT
        self.schedule.add(agent)
        self.counter +=1
        self.gridRoad.place_agent(agent,(3,1))
        agent = CarAgent(self.counter, self)
        agent.direction = Direction.UP
        self.schedule.add(agent)
        self.counter +=1
        self.gridRoad.place_agent(agent,(2,0))
    def step(self):
        self.schedule.step()


# In[ ]:





# In[815]:


#import mesa
#
#
#def agent_portrayal(RoadAgent):
#    portrayal = {"Shape": "circle",
#                 "Filled": "true",
#                 "Layer": 0,
#                 "Color": "red",
#                 "r": 0.5}
#    return portrayal
#model = RoadModel(myMap)
#grid = mesa.visualization.CanvasGrid(agent_portrayal, 5, 4, 400, 400)
#server = mesa.visualization.ModularServer(RoadModel, [grid], "Road Model", {"roadMap": myMap})
#
#server.port = 8501 # The default
#server.launch()


# In[816]:


model = RoadModel(myMap)


# In[817]:


for i in range(8):
    model.step()


# In[811]:


model.step()


# In[818]:


for i in range(4):
    display(model.gridRoad[i])


# In[803]:


for i in model.schedule.agents:
    if isinstance(i,CarAgent):
        print(i.direction)


# In[ ]:





# In[ ]:




