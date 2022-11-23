using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    // Variable that we will use to moe and steer our car:
    public float acceleration = 5000f;
    public float brakesDecceleration = 500f;
    public float steerAngle = 20f;

    private float currentAcceleration = 0f;
    private float currentDecceleration = 0f;
    private float currentSteerAngle = 0f;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    // As we will used the Wheel Collider to accelerate and to steer, let's add the Wheel colliders of our car.
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider rearRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider rearLeft;


    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        currentAcceleration = acceleration * verticalInput * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            currentDecceleration = brakesDecceleration;
        }
        else
        {
            currentDecceleration = 0f;
        }

        // Car acceleration and decelaration
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentDecceleration;
        frontLeft.brakeTorque = currentDecceleration;
        rearRight.brakeTorque = currentDecceleration;
        rearLeft.brakeTorque = currentDecceleration;

        // Car steering
        currentSteerAngle = steerAngle * horizontalInput;
        frontLeft.steerAngle = currentSteerAngle;
        frontRight.steerAngle = currentSteerAngle;
    }


    private void MoveForward()
    {

    }

    private void Turn() 
    {

    }

    private void Slowdown()
    {
        currentSteerAngle = 0f;
        currentDecceleration = 500f;
    }
}