using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Player Movement")]
    private Vector2 moveInput;
    private Vector3 movementDirection;
    private Vector3 verticalMovement;
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _verticalVelocity;

    [Header("Player Aim")]
    private Vector2 aimInput;
    private Vector3 lookDirection;
    [SerializeField] private Transform crossHair;
    [SerializeField] private LayerMask _aimLayerMask;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => aimInput = Vector2.zero;
    }

    private void Update()
    {
        Movement();
        Look();
        AnimatorControl();
    }

    private void Look()
    {
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
        {
            lookDirection = hitInfo.point - transform.position;
            lookDirection.y = 0;
            lookDirection.Normalize();

            transform.forward = lookDirection;

            if (crossHair != null)
                crossHair.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
        }
    }

    private void AnimatorControl()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);
    }

    private void Movement()
    {
        ApplyGravity();

        // Horizontal movement
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        if (movementDirection.magnitude > 0)
        {
            // Transform movement direction based on character orientation
            movementDirection = transform.TransformDirection(movementDirection);
            characterController.Move(_walkSpeed * Time.deltaTime * movementDirection);
        }

        // Apply vertical movement
        characterController.Move(verticalMovement * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
            _verticalVelocity = -0.5f;
        else
            _verticalVelocity -= 9.81f * Time.deltaTime;
        verticalMovement = new Vector3(0, _verticalVelocity, 0);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
