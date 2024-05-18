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

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void Update()
    {
        Movement();
        AnimatorControl();
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
