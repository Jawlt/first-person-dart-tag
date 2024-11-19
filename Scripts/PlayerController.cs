using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    public bool canMove = true;
    public bool isPaused = false; // New variable to track if the game is paused
    public bool isIt;

    private CharacterController characterController;
    private Animator animator;

    // Sprinting and walking states
    private bool isRunning = false;
    private bool isMoving = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Get the Animator component
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isPaused) return; // Prevent movement if the game is paused

        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Check if Left Shift is pressed for running
        isRunning = Input.GetKey(KeyCode.LeftShift) && canMove;

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;

        // Determine move direction based on input
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Check if the player is moving (ignoring vertical movement)
        isMoving = moveDirection.x != 0 || moveDirection.z != 0;

        // Update animator states
        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isMoving", isMoving);
        }

        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion

        #region Handles Rotation
        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion
    }

    // Public method to set the "it" status, can be called from GameManager
    public void SetIt(bool status)
    {
        isIt = status;
    }
}
