using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CarMovement : NetworkBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] float speed = 5f;
    [SerializeField] float brakeSpeed = 3f;

    [Header("Steering Variables")]
    [SerializeField] float maxSteerAngle = 30f;
    [SerializeField] float steerSpeed = 3f;
    [SerializeField] float steerReturnSpeed = 2f;
    [SerializeField] float turnSpeed = 3f;

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle = 0f;
    private Rigidbody rb;

    [Header("Wheels")]
    [SerializeField] Transform RWheel, LWheel;  // Steering wheels (Front wheels)
    [SerializeField] float WheelXROtMultiplier = 0;
    float WheelXROt = 0;

    void Start()
    {
      //  if (!IsOwner) return;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; //make sure kinematic is false in multiplayer
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Helps smooth physics movement
    }

    //public override void OnNetworkSpawn()
    //{
    //  //  if (!IsOwner) return;
    //       // Rigidbody rb = GetComponent<Rigidbody>();
    //        rb.isKinematic = false;
    //        rb.interpolation = RigidbodyInterpolation.Interpolate;
    //}


    void Update()
    {
      //  if (!IsOwner) return;

        GetInput();
        Move();
        RotateWheels();
        Steer();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void Move()
    {
        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            rb.AddForce(transform.forward * speed * verticalInput * Time.deltaTime, ForceMode.Force);
        }     
    }

    void Steer()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            if (Mathf.Abs(horizontalInput) > 0.1f)
            {
                currentSteerAngle = Mathf.Lerp(currentSteerAngle, maxSteerAngle * horizontalInput, steerSpeed * Time.deltaTime);
            }
            else
            {
                currentSteerAngle = Mathf.Lerp(currentSteerAngle, 0f, steerReturnSpeed * Time.deltaTime);
            }

            // Apply steering to front wheels
            RWheel.localRotation = Quaternion.Euler(WheelXROt, currentSteerAngle/10, 0f);
            LWheel.localRotation = Quaternion.Euler(WheelXROt, currentSteerAngle/10, 0f);

            // Apply smooth car rotation
            Quaternion targetRotation = Quaternion.Euler(0f, currentSteerAngle * (rb.velocity.magnitude / (speed * 0.004f)), 0f);
            print(rb.velocity.magnitude);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, rb.rotation * targetRotation, turnSpeed * Time.deltaTime));
        }
    }

    void RotateWheels()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            WheelXROt += transform.InverseTransformDirection(rb.velocity).z * WheelXROtMultiplier * Time.deltaTime;
        }
    }
}
