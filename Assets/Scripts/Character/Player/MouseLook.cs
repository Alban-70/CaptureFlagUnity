using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSensitivity = 1f;

    [Header("References")]
    public Transform playerBody; // Le corps à faire tourner horizontalement

    private PlayerInputActions inputActions;
    private Vector2 lookInput;
    private float xRotation = 0f;

    // Direction actuelle de la caméra
    public Vector3 CameraForward => transform.forward;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLookCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLookCanceled;
        inputActions.Player.Disable();
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        lookInput = Vector2.zero;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // --- Rotation caméra et joueur ---
        float mouseY = lookInput.y * mouseSensitivity;

        // Rotation verticale (caméra)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
