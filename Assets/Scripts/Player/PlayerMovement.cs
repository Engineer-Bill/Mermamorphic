using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D _rb;
    private Vector2 _movementVelocity;

    private bool _canMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_canMove) _rb.MovePosition(_rb.position + _movementVelocity * (speed * Time.fixedDeltaTime));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movementVelocity = context.ReadValue<Vector2>();
            _canMove = true;
        }
        else if (context.canceled)
        {
            _movementVelocity = Vector2.zero;
            _canMove = false;
        }
    }
}
