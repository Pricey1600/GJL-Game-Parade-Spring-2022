using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VanController : MonoBehaviour
{
    public bool gameStarted = false;
    private float steerDirection, steerAngle, currentSteerAngle, currentSpeed, currentBreakForce;
    private bool isBreaking, isReversing, isReadyToReverse;

    [SerializeField] private float motorForce, reverseForce, breakForce, maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;

    [SerializeField] private Rigidbody rb;
    private float vanMovingPitch;

    private float resetCooldownTimer, completeResetTimer;
    [SerializeField] private float resetCooldownDuration, completeResetHoldDuration;
    private bool completelyResetting;

    [SerializeField] private AudioClip vanIdle, vanMoving;
    [SerializeField] private AudioSource vanAudioSource, vanHornAS;

    private void OnEnable()
    {
        GameManager.OnStarted += GameStart;
        GameManager.OnComplete += GameEnd;
    }

    private void OnDisable()
    {
        GameManager.OnStarted -= GameStart;
        GameManager.OnComplete -= GameEnd;
    }
    private void GameStart()
    {
        gameStarted = true;
        isReversing = false;
        completeResetTimer = completeResetHoldDuration;
        //enable camera movement
    }
    private void GameEnd()
    {
        gameStarted = false;
        //disable camera movement
    }
    private void Update()
    {
        if(resetCooldownTimer > 0)
        {
            resetCooldownTimer -= Time.deltaTime;
        }
        if(completeResetTimer > 0 && completelyResetting)
        {
            completeResetTimer -= Time.deltaTime;
            if(completeResetTimer <= 0)
            {
                //reset van fully
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.position = new Vector3(0, 1.5f, 0);
            }
            
        }
    }
    private void FixedUpdate()
    {
        if (gameStarted)
        {
            handleMotor();
            handleSteering();
            updateWheels();
        }
        else
        {
            currentBreakForce = breakForce;
            applyBreaking();
        }

        vanMovingPitch = rb.velocity.magnitude / 10f;
        vanAudioSource.pitch = Mathf.Clamp(vanMovingPitch, 0.5f, 1);
        
    }

    private void handleMotor()
    {
        if (!isReversing)
        {
            frontLeftWheelCollider.motorTorque = currentSpeed * motorForce;
            frontRightWheelCollider.motorTorque = currentSpeed * motorForce;
            backLeftWheelCollider.motorTorque = currentSpeed * motorForce;
            backRightWheelCollider.motorTorque = currentSpeed * motorForce;
        }
        else if (isReversing)
        {
            frontLeftWheelCollider.motorTorque = currentSpeed * reverseForce;
            frontRightWheelCollider.motorTorque = currentSpeed * reverseForce;
            backLeftWheelCollider.motorTorque = currentSpeed * reverseForce;
            backRightWheelCollider.motorTorque = currentSpeed * reverseForce;
        }
        

        if (isBreaking && !isReversing)
        {
            if(rb.velocity.magnitude < 2f)
            {
                isReadyToReverse = true;
                //if (vanAudioSource.clip != vanIdle)
                //{
                //    vanAudioSource.clip = vanIdle;
                //    vanAudioSource.Play();
                //}
                
            }
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
        isReversing = false;
        isReadyToReverse = false;
        currentSpeed = context.ReadValue<float>();
        //if (vanAudioSource.clip != vanMoving)
        //{
        //    vanAudioSource.clip = vanMoving;
        //    vanAudioSource.Play();
        //}
        

    }
    public void Decelerate(InputAction.CallbackContext context)
    {
        var decelerateFloat = context.ReadValue<float>();
        if(decelerateFloat != 0 && !isReadyToReverse)
        {
            isBreaking = true;
        }
        else if(context.performed && isReadyToReverse)
        {
            isReversing = true;
            isBreaking = false;
            currentSpeed = decelerateFloat;
        }
        else
        {
            isBreaking = false;
            //isReversing = false;
        }
    }

    public void ResetVan(InputAction.CallbackContext context)
    {
        if(resetCooldownTimer <= 0 && completeResetTimer == completeResetHoldDuration)
        {
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            resetCooldownTimer = resetCooldownDuration;
        }
        if (context.performed && completeResetTimer > 0)
        {
            
            completelyResetting = true;
        }
        else
        {
            completelyResetting = false;
            completeResetTimer = completeResetHoldDuration;
        }
        
    }

    public void SoundHorn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            vanHornAS.Play();
        }
        else
        {
            vanHornAS.Stop();
        }
        
    }



}
