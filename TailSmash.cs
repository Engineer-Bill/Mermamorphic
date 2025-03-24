using UnityEngine;
using UnityEngine.Tilemaps;

public class TailSmash : MonoBehaviour
{
    public float smashRadius = 1.5f; // The radius of the attack
    public LayerMask smashableLayers; // Set this to include "BreakableWall"
    public float smashCooldown = 1f; // Cooldown between smashes

    private bool canSmash = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canSmash) // Change key if needed
        {
            PerformSmash();
        }
    }

    void PerformSmash()
    {
        canSmash = false;
        Debug.Log("Tail Smash!");

        // Detect objects within the smash radius
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, smashRadius, smashableLayers);

        foreach (Collider2D obj in hitObjects)
        {
            Tilemap tilemap = obj.GetComponent<Tilemap>(); // Check if it's a Tilemap
            if (tilemap != null)
            {
                // Call the BreakTilesInRadius function to break all tiles within the radius
                tilemap.GetComponent<BreakableWall>()?.BreakTilesInRadius(transform.position, smashRadius);
            }
        }

        // Cooldown before next smash
        Invoke(nameof(ResetSmash), smashCooldown);
    }



    void ResetSmash()
    {
        canSmash = true;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, smashRadius);

        // Draw a visual indicator of the current smash position
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.1f); // This is the position being checked for smashing
    }

}
