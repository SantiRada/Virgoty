using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Sprite Settings")]
    private SpriteRenderer spriteRenderer;

    private Vector2 moveInput;
    private Camera mainCamera;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        spriteRenderer = GetComponent<SpriteRenderer>();

        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }
    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Disable();
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void Update()
    {
        HandleMovement();
        HandleSpriteFlip();
    }
    private void HandleMovement()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }
    private void HandleSpriteFlip()
    {
        if (mainCamera != null)
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorldPos.z = 0f;

            // Flip sprite según la posición del mouse
            if (mouseWorldPos.x < transform.position.x)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }
    }
    public Vector3 GetMouseWorldPosition()
    {
        if (mainCamera != null)
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseWorldPos.z = 0f;
            return mouseWorldPos;
        }
        return Vector3.zero;
    }
    private void OnDestroy()
    {
        inputActions?.Dispose();
    }
}