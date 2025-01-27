using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class playerController : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float sensetivity;
    [SerializeField] private float rotateSensetivity;
    [SerializeField] private float brakingSpeed;
    [SerializeField] private float stoppingSpeed;
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener listner;


    private Camera playerCamera;
    private Vector3 acceleration;
    float mouseX;
    private float pitch = 0f; // Vertical camera rotation

    // Start is called before the first frame update
    void Start()
    {
        acceleration = Vector3.zero;
        playerCamera = Camera.main;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listner.enabled = true;
            vc.Priority = 1;
        }
        else
        {
            vc.Priority = 0;
        }
    }

    void Update()
    {
        // Get the mouse input on the X-axis (horizontal movement) and Y-axis (vertical movement)
        mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the camera vertically based on mouse Y input (up/down)
        pitch -= mouseY * 20f * Time.deltaTime; // Control vertical rotation (look up/down)
        pitch = Mathf.Clamp(pitch, -90f, 90f);  // Prevent flipping upside down
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f); // Apply vertical rotation to the camera

        // Rotate the player horizontally based on mouse X input (left/right)
        
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * mouseX * rotateSensetivity * Time.deltaTime); // Rotate object horizontally

        float moveHorizontal;
        float moveVertical;

        // Handle brake input (Jump)
        if (Input.GetAxis("Jump") > 0)
        {
            // Get the current movement direction based on velocity
            Vector3 currentVelocity = rb.velocity;

            // Calculate the current movement direction on the XZ plane (ignoring the vertical component)
            Vector3 currentMovementDirection = new Vector3(currentVelocity.x, 0f, currentVelocity.z).normalized;

            // Apply opposing force to decelerate
            if (currentMovementDirection.sqrMagnitude > 0.01f) // Ensure we are moving before applying brakes
            {
                Vector3 opposingForce = -currentMovementDirection * brakingSpeed; // Apply force in the opposite direction (adjust the multiplier as needed)
                rb.AddForce(opposingForce, ForceMode.Force);
            }
        }
        else
        {
            // Get input for movement (Horizontal = A/D or Left/Right, Vertical = W/S or Up/Down)
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");

            if(moveHorizontal == 0 && moveVertical == 0)
            {
                // Get the current movement direction based on velocity
                Vector3 currentVelocity = rb.velocity;

                // Calculate the current movement direction on the XZ plane (ignoring the vertical component)
                Vector3 currentMovementDirection = new Vector3(currentVelocity.x, 0f, currentVelocity.z).normalized;

                // Apply opposing force to decelerate
                if (currentMovementDirection.sqrMagnitude > 0.01f) // Ensure we are moving before applying brakes
                {
                    Vector3 opposingForce = -currentMovementDirection * stoppingSpeed; // Apply force in the opposite direction (adjust the multiplier as needed)
                    rb.AddForce(opposingForce, ForceMode.Force);
                }
            }
            else { 
                // Get the camera's forward and right directions, but ignore the y-axis (vertical) component for horizontal movement
                Vector3 cameraForward = playerCamera.transform.forward;
                Vector3 cameraRight = playerCamera.transform.right;

                // Flatten the forward vector to only use the x and z components (ignore y-axis tilt)
                cameraForward.y = 0f;
                cameraRight.y = 0f;

                // Normalize the vectors to avoid scaling issues when adding them
                cameraForward.Normalize();
                cameraRight.Normalize();

                // Calculate the movement direction based on the camera's rotation
                Vector3 moveDirection = cameraForward * moveVertical + cameraRight * moveHorizontal;

                // Apply the force in the direction of movement relative to the camera's orientation
                rb.AddForce(moveDirection * 10f, ForceMode.Force); // Adjust multiplier (10f) for speed
            }
        }

        // Rotate the player (object) to face the camera's forward direction on the Y-axis
        Vector3 targetDirection = playerCamera.transform.forward; // Use the camera's forward direction
        targetDirection.y = 0f; // Keep the target direction flat on the XZ plane

        if (targetDirection.sqrMagnitude > 0.05f) // Check if the target direction is valid
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection); // Calculate the rotation to face the target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, sensetivity * Time.deltaTime); // Smoothly rotate towards the target direction
        }
    }


}
