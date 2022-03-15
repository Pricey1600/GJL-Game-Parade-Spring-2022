using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VanController : MonoBehaviour
{
    public bool gameStarted = false;
    private float steerDirection, steerAngle, currentSteerAngle, currentSpeed, currentBreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce, minSpeed, breakForce, maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //if (gameStarted)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}
        //may change out for check velocity function. If falls below set speed player loses
        if(currentSpeed < minSpeed && gameStarted)
        {
            currentSpeed = minSpeed;
        }
    }
    private void FixedUpdate()
    {
        handleMotor();
        handleSteering();
        updateWheels();
    }

    private void handleMotor()
    {
        frontLeftWheelCollider.motorTorque = currentSpeed * motorForce;
        frontRightWheelCollider.motorTorque = currentSpeed * motorForce;

        if (isBreaking)
        {
            currentBreakForce = breakForce;
            applyBreaking();
        }
        else
        {
            currentBreakForce = 0f;
            applyBreaking();
        }
    }
    private void applyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        backRightWheelCollider.brakeTorque = currentBreakForce;
        backLeftWheelCollider.brakeTorque = currentBreakForce;
    }

    private void handleSteering()
    {
        currentSteerAngle = maxSteerAngle * steerDirection;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void updateWheels()
    {
        updateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        updateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        updateSingleWheel(backLeftWheelCollider, backLeftWheelTransform);
        updateSingleWheel(backRightWheelCollider, backRightWheelTransform);
    }

    private void updateSingleWheel(WheelCollider WheelCollider, Transform WheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        WheelCollider.GetWorldPose(out pos, out rot);
        WheelTransform.rotation = rot;
        WheelTransform.position = pos;
    }

    public void Steer(InputAction.CallbackContext context)
    {
        steerDirection = context.ReadValue<float>();
    }

    public void Accelerate(InputAction.CallbackContext context)
    {
        currentSpeed = context.ReadValue<float>();
    }
    public void Decelerate(InputAction.CallbackContext context)
    {
        var decelerateFloat = context.ReadValue<float>();
        if(decelerateFloat != 0)
        {
            isBreaking = true;
        }
        else
        {
            isBreaking = false;
        }
    }
}
