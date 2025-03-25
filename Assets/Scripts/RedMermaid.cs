using UnityEngine;
using UnityEngine.InputSystem;
public class RedMermaid : MonoBehaviour
{
    private bool _canAttack;
    private GameObject _breakableWall;
    private Switch _switch;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BreakableWall"))
        {
            _canAttack = true;
            _breakableWall = other.gameObject;
        }
        else if (other.CompareTag("Switch"))
        {
            _switch = other.GetComponent<Switch>();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        _canAttack = false;
        _switch = null;
    }
    public void OnAbilityUse(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (_switch != null)
        {
            _switch.ToggleSwitch();
        }
        else if (_breakableWall != null)
        {
            _breakableWall.SetActive(false);
        }
    }
}