using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintMultiplier = 1.6f;
    public float crouchMultiplier = 0.45f;
    public float rotationSpeed = 12f;

    [Header("Gravity")]
    public float gravity = -9.8f;

    [Header("Crouch")]
    public float standingHeight = 1.8f;
    public float crouchHeight = 1.0f;
    public float crouchTransitionSpeed = 10f;

    public Transform cameraTransform;

    CharacterController controller;
    PlayerInput input;

    Vector2 moveInput;
    bool isRunning;
    bool isCrouching;
    float verticalVelocity;
    float targetHeight;
void Start()
{
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = new PlayerInput();

        targetHeight = standingHeight;

        input.CharacterControls.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.CharacterControls.Move.canceled += _ => moveInput = Vector2.zero;

        input.CharacterControls.Run.started += _ => isRunning = true;
        input.CharacterControls.Run.canceled += _ => isRunning = false;

        input.CharacterControls.Crouch.started += _ => isCrouching = true;
        input.CharacterControls.Crouch.canceled += _ => isCrouching = false;
    }

    void Update()
    {
        HandleGravity();
        HandleCrouch();
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir =
            camForward * moveInput.y +
            camRight * moveInput.x;

        float speed = walkSpeed;

        if (isRunning && !isCrouching && moveInput.y > 0)
            speed *= sprintMultiplier;

        if (isCrouching)
            speed *= crouchMultiplier;

        controller.Move((moveDir * speed + Vector3.up * verticalVelocity) * Time.deltaTime);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void HandleCrouch()
    {
        targetHeight = isCrouching ? crouchHeight : standingHeight;

        controller.height = Mathf.Lerp(
            controller.height,
            targetHeight,
            crouchTransitionSpeed * Time.deltaTime
        );

        controller.center = new Vector3(0, controller.height / 2f, 0);
    }

    void HandleGravity()
    {
        if (controller.isGrounded)
            verticalVelocity = -2f;
        else
            verticalVelocity += gravity * Time.deltaTime;
    }

    void OnEnable() => input.CharacterControls.Enable();
    void OnDisable() => input.CharacterControls.Disable();
}
