using UnityEngine;
using UnityEngine.InputSystem;

public class RedMermaid : MonoBehaviour
{
    private Vector2 _aim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAim(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        _aim = context.ReadValue<Vector2>();
        Debug.Log(_aim);
    }

    public void OnAbilityUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }
}
