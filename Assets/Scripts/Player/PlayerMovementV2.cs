using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovementV2 : MonoBehaviour
{
    public float defaultSpeed = 5.5f,runningSpeed = 8.5f,jumpForce = 8.0f,gravity = 9.81f,lookSpeed = 2.0f,lookXLimit = 45.0f,rotationX = 0;

    public Camera playerCamera;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;

    [HideInInspector]
    public bool canMove = true; 

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speedX = canMove ? (isRunning ? runningSpeed : defaultSpeed) * Input.GetAxis("Vertical") : 0;
        float speedY = canMove ? (isRunning ? runningSpeed : defaultSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * speedX) + (right * speedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpForce;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
} 