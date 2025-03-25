using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool isOn = false; // Track switch state
    public GameObject door;   // Reference to the door
    public LineRenderer trailRenderer; // Reference to the line renderer

    public Sprite switchOnSprite;  // Assign the ON sprite in Inspector
    public Sprite switchOffSprite; // Assign the OFF sprite in Inspector
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        UpdateSwitchVisual(); // Set initial sprite
    }

    public void ToggleSwitch()
    {
        isOn = !isOn; // Toggle the state
        UpdateSwitchVisual(); // Update the sprite

        if (isOn)
            TurnOn();
        else
            TurnOff();
    }

    private void TurnOn()
    {
        Debug.Log("Switch is ON!");
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
            trailRenderer.SetPosition(0, transform.position);
            trailRenderer.SetPosition(1, door.transform.position);
        }

        if (door != null)
            door.GetComponent<Door>().OpenDoor();
    }

    private void TurnOff()
    {
        Debug.Log("Switch is OFF!");
        if (trailRenderer != null)
            trailRenderer.enabled = false;

        if (door != null)
            door.GetComponent<Door>().CloseDoor();
    }

    private void UpdateSwitchVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isOn ? switchOnSprite : switchOffSprite;
        }
    }
}
