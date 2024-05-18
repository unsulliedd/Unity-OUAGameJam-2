using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    [SerializeField] private CharacterController characterController;

    [SerializeField]private Vector2 moveInput;
    [SerializeField]private Vector3 movementDirection;

    [SerializeField] private float _walkSpeed = 10f;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        if (movementDirection.magnitude > 0)
        {
            characterController.Move(_walkSpeed * Time.deltaTime * movementDirection);
        }
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
