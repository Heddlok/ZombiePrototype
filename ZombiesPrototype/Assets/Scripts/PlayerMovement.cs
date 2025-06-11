using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 7f;
    public float sprintSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    private PlayerInputActions inputActions;
    private CharacterController controller;
    private Vector2 moveInput;
    private bool isSprinting;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // Enable the Player action map
        inputActions.Player.Enable();

        // Hook up Move started/performed/canceled
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;

        inputActions.Player.Sprint.performed += ctx => isSprinting = true;
        inputActions.Player.Sprint.canceled += ctx => isSprinting = false;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled  -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Sprint.performed -= ctx => isSprinting = true;
        inputActions.Player.Sprint.canceled -= ctx => isSprinting = false;
        inputActions.Player.Disable();
    }

    // Callback for both started/performed (gives non-zero) and canceled (gives zero)
    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (controller.isGrounded)
        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calculate jump velocity
    }

    private void Update()
    {
        float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;
        // Calculate horizontal movement in local space
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= moveSpeed;

        // Simple gravity
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;    // small downward force to keep grounded

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        // Actually move the character
        controller.Move(move * Time.deltaTime);
    }
}
