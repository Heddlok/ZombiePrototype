using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Look sensitivity in degrees per second")]
    public float mouseSensitivity = 100f;

    [Tooltip("Clamp pitch between these angles")]
    public float topClamp = -90f;
    public float bottomClamp = 90f;

    // runtime state
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector2 lookInput = Vector2.zero;

    // Input System
    private PlayerInputActions inputActions;

    void Awake()
    {
        // instantiate the generated C# class
        inputActions = new PlayerInputActions();

        // hook up the Look action
        inputActions.Player.Look.performed += ctx => 
            lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => 
            lookInput = Vector2.zero;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        // lock & hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // apply sensitivity & frame delta
        Vector2 delta = lookInput * mouseSensitivity * Time.deltaTime;

        // Yaw (around Y axis)
        yRotation += delta.x;

        // Pitch (around X axis, clamped)
        xRotation -= delta.y;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // apply to transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}