using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RedMermaid : MonoBehaviour
{
    private Vector2 _aim;
    public Transform headTransform;

    private bool _canAttack;
    public float attackLength = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (!_canAttack) return;
        var hit = Physics2D.Raycast(headTransform.position, Vector2.right * attackLength);
        if (hit.collider.CompareTag("BreakableWall"))
        {
            Destroy(hit.collider.gameObject);
        }
    }

    public void GetAim(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        _aim = context.ReadValue<Vector2>();
        Debug.Log(_aim);
    }

    public void OnAbilityUse(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        _canAttack = true;
        StartCoroutine(CancelAttack());
    }

    IEnumerator CancelAttack()
    {
        yield return new WaitForSeconds(0.5f);
        _canAttack = false;
    }
}
