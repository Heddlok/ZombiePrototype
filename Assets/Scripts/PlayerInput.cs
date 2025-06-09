using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputNew : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float mouseSensitivity = 3.0f; // Adjust for preference

    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    public PlayerInputActions playerInputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;

    private float xRotation = 0f; // Camera pitch
    public Transform cameraTransform; // Assign your camera here

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        // Movement binding
        playerInputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Look binding
        playerInputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        // Jump binding
        playerInputActions.Player.Jump.performed += ctx => jumpPressed = true;
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController missing. Add one in the Inspector.");
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Hide and lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (controller == null) return;

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -1f;
        }

        // Mouse look
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movement
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move); // Convert local to world space
        move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * Time.deltaTime * playerSpeed);

        // Jump
        if (jumpPressed && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            jumpPressed = false;
        }

        // Gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);
    }
}
