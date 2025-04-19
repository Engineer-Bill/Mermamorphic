using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    // In degrees
    [SerializeField]
    private float _rotationSpeed = 180f;
    Rigidbody2D _rb;
    private Vector2 _movementVelocity;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Animator _animator;

    private bool _canMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 currentFacing = transform.TransformDirection(Vector2.up);
        Vector2 targetFacing = Vector2.up;
        if (_movementVelocity != Vector2.zero)
        {
            targetFacing = _movementVelocity.normalized;
        }
        float desiredRotation = Vector2.SignedAngle(currentFacing, targetFacing);
        float scaledRotation =
            Mathf.Clamp(desiredRotation, -_rotationSpeed * Time.fixedDeltaTime, _rotationSpeed * Time.fixedDeltaTime);

        float currentAngle = Vector2.SignedAngle(Vector2.up, currentFacing);

        _rb.MovePositionAndRotation(
            _rb.position + _movementVelocity * (_speed * Time.fixedDeltaTime),
            currentAngle + scaledRotation);
        if (_movementVelocity.SqrMagnitude() < 0.01f)
        {
            _animator.SetBool("Swimming", false);
        } else
        {
            _animator.SetBool("Swimming", true);
        }
    }

    public void StopMoving()
    {
        _movementVelocity = Vector2.zero;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movementVelocity = context.ReadValue<Vector2>();
            _canMove = true;
            _spriteRenderer.flipX = _movementVelocity.x switch
            {
                < 0 => true,
                > 0 => false,
                _ => _spriteRenderer.flipX
            };
        }
        else if (context.canceled)
        {
            _movementVelocity = Vector2.zero;
            _canMove = false;
        }
    }
}
