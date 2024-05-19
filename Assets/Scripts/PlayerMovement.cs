using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Components")]
    private PlayerControls controls;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Player Movement")]
    private Vector2 moveInput;
    private Vector3 movementDirection;
    private Vector3 verticalMovement;
    private bool isRunning;
    private float _speed;
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _verticalVelocity;

    [Header("Player Aim")]
    private Vector2 aimInput;
    private Vector3 lookDirection;
    [SerializeField] private Transform crossHair;
    [SerializeField] private LayerMask _aimLayerMask;
    [SerializeField] private float minAimDistance = 1f;  // Minimum distance to avoid spinning

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => aimInput = Vector2.zero;

        controls.Player.Run.performed += ctx => { isRunning = true; _speed = _runSpeed; };
        controls.Player.Run.canceled += ctx => { isRunning = false; _speed = _walkSpeed; };

        controls.Player.Fire.performed += ctx => Fire();
    }

    void Start()
    {
        _speed = _walkSpeed;
    }

    private void Update()
    {
        Movement();
        Look();
        AnimatorControl();
    }

    private void Fire()
    {
        animator.SetTrigger("Fire");
    }

    private void Look()
    {
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
        {
            lookDirection = hitInfo.point - transform.position;
            float distance = lookDirection.magnitude;

            if (distance > minAimDistance)
            {
                lookDirection.y = 0;
                lookDirection.Normalize();
                transform.forward = lookDirection;
            }

            if (crossHair != null)
                crossHair.position = new Vector3(hitInfo.point.x, crossHair.position.y, hitInfo.point.z);
        }
    }

    private void AnimatorControl()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

        bool playRunAnimation = isRunning && moveInput.magnitude > 0;
        animator.SetBool("isRunning", playRunAnimation);
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
            characterController.Move(_speed * Time.deltaTime * movementDirection);
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
