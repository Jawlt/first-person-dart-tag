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
    public bool isPaused = false;
    public bool isIt;

    private CharacterController characterController;
    private Animator animator;
    private Gun gun;
    private bool isRunning = false;
    private bool isMoving = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gun = GetComponentInChildren<Gun>();
        if (gun != null)
        {
            gun.isIt = isIt;
        }
    }

    void Update()
    {
        if (isPaused) return;

        HandleMovement();
        HandleJumping();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        isRunning = Input.GetKey(KeyCode.LeftShift) && canMove;

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        isMoving = moveDirection.x != 0 || moveDirection.z != 0;

        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isMoving", isMoving);
        }

        moveDirection.y = movementDirectionY;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleJumping()
    {
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }

    void HandleRotation()
    {
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    public void SetIt(bool status)
    {
        isIt = status;

        if (gun != null)
        {
            gun.isIt = status;
        }

        Debug.Log($"Player is now {(isIt ? "it" : "not it")}!");
    }
}
