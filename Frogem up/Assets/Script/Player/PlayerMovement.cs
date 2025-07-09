using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer = -1;

    [Header("Rotation Settings")]
    public GameObject rotationTarget;
    public float rotationSpeed = 10f;
    public bool rotateTowardsMouse = true;
    public bool rotateTowardsMovement = false;

    [Header("3D Settings")]
    public float raycastDistance = 100f;

    private Rigidbody controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Camera mainCamera;
    [HideInInspector] public PlayerInputActions inputActions;
    private bool isGrounded;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        mainCamera = Camera.main;

        controller = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
    }
    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Disable();
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            controller.velocity = new Vector3(controller.velocity.x, jumpSpeed, controller.velocity.z);
        }
    }
    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, transform.localScale.y / 2 + groundCheckDistance, groundLayer);

        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        move = transform.TransformDirection(move);

        controller.velocity = new Vector3(move.x, controller.velocity.y, move.z);
    }
    private void HandleRotation()
    {
        if (rotateTowardsMouse) { RotateTowardsMouse(); }
        else if (rotateTowardsMovement && moveInput.magnitude > 0.1f) { RotateTowardsMovement(); }
    }
    private void RotateTowardsMouse()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        Vector3 directionToMouse = (mouseWorldPos - transform.position).normalized;
        directionToMouse.y = 0f;

        if (directionToMouse.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToMouse);
            rotationTarget.transform.rotation = Quaternion.Slerp(rotationTarget.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void RotateTowardsMovement()
    {
        Vector3 movementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        if (movementDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            rotationTarget.transform.rotation = Quaternion.Slerp(rotationTarget.transform.rotation, targetRotation,
                                                rotationSpeed * Time.deltaTime);
        }
    }
    public Vector3 GetMouseWorldPosition()
    {
        if (mainCamera != null)
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mouseScreenPos);

            // Raycast contra el suelo/superficie
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, groundLayer))
            {
                return hit.point;
            }

            // Si no hay hit, proyectar en un plano a la altura del jugador
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            if (playerPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            // Fallback - proyectar a distancia fija
            return ray.GetPoint(10f);
        }
        return Vector3.zero;
    }
    private void OnDestroy()
    {
        inputActions?.Dispose();
    }
}