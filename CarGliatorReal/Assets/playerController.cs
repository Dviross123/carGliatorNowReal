using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private Camera playerCamera;
    private Vector3 acceleration;

    private float pitch = 0f; // Vertical camera rotation

    // Start is called before the first frame update
    void Start()
    {
        acceleration = Vector3.zero;
        playerCamera = Camera.main;
    }

    void Update()
    {
        // Get the mouse input on the X-axis (horizontal movement) and Y-axis (vertical movement)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the camera vertically based on mouse Y input (up/down)
        pitch -= mouseY * 20f * Time.deltaTime; // Control vertical rotation (look up/down)
        pitch = Mathf.Clamp(pitch, -90f, 90f);  // Prevent flipping upside down
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f); // Apply vertical rotation to the camera

        // Rotate the player horizontally based on mouse X input (left/right)
        transform.Rotate(Vector3.up * mouseX * 20f * Time.deltaTime); // Rotate object horizontally
    }

    private void FixedUpdate()
    {
        float moveHorizontal;
        float moveVertical;

        // Handle brake input (Jump)
        if (Input.GetAxis("Jump") > 0)
        {
            print("brakes");

            // Get the current movement direction based on velocity
            Vector3 currentVelocity = rb.velocity;

            // Calculate the current movement direction on the XZ plane (ignoring the vertical component)
            Vector3 currentMovementDirection = new Vector3(currentVelocity.x, 0f, currentVelocity.z).normalized;

            // Apply opposing force to decelerate
            if (currentMovementDirection.sqrMagnitude > 0.01f) // Ensure we are moving before applying brakes
            {
                Vector3 opposingForce = -currentMovementDirection * 20f; // Apply force in the opposite direction (adjust the multiplier as needed)
                rb.AddForce(opposingForce, ForceMode.Force);
            }
        }
        else
        {
            // Get input for movement (Horizontal = A/D or Left/Right, Vertical = W/S or Up/Down)
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");

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

        // Rotate the player (object) to face the camera's forward direction on the Y-axis
        Vector3 targetDirection = playerCamera.transform.forward; // Use the camera's forward direction
        targetDirection.y = 0f; // Keep the target direction flat on the XZ plane

        if (targetDirection.sqrMagnitude > 0.05f) // Check if the target direction is valid
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection); // Calculate the rotation to face the target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime); // Smoothly rotate towards the target direction
        }
    }


}