using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player input for movement, look, and jump using Rigidbody-based physics.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerInput : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed in units per second")]
    public float playerSpeed = 5f;

    [Header("Jump Settings")]
    [Tooltip("Height of jump in units")]
    public float jumpHeight = 2f;
    [Tooltip("Ground check transform for jumping")]
    public Transform groundCheck;
    [Tooltip("Radius for ground check sphere")]
    public float groundDistance = 0.2f;
    [Tooltip("Layers considered as ground")]
    public LayerMask groundMask;

    [Header("Look Settings")]
    [Tooltip("Mouse look sensitivity")]
    public float mouseSensitivity = 14f;
    [Tooltip("Reference to the camera for pitch")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;
    private float xRotation;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Freeze all physics rotations so we control yaw manually
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled  += ctx => moveInput = Vector2.zero;
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled  += ctx => lookInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => jumpPressed = true;
    }

    private void OnEnable()  => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void OnDestroy()
    {
        // Ensure callbacks and asset are cleaned up
        inputActions.Player.Disable();
        inputActions.Dispose();
    }

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Camera pitch (up/down) – do this in Update for tight responsivity
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Ground check for jumping, guard against missing transform
        isGrounded = groundCheck != null && Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void FixedUpdate()
    {
        // Body yaw (left/right) – drive Rigidbody in FixedUpdate
        float mouseX = lookInput.x * mouseSensitivity * Time.fixedDeltaTime;
        Quaternion turn = Quaternion.Euler(0f, mouseX, 0f);
        rb.MoveRotation(rb.rotation * turn);

        // Movement
        Vector3 localMove = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 worldMove = transform.TransformDirection(localMove);
        rb.MovePosition(rb.position + worldMove * playerSpeed * Time.fixedDeltaTime);

        // Jump
        if (jumpPressed && isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            rb.AddForce(Vector3.up * jumpVelocity, ForceMode.VelocityChange);
            jumpPressed = false;
        }
    }
}
