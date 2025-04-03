using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakableWall : MonoBehaviour
{
    private Tilemap tilemap;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>(); // Ensure this grabs the correct Tilemap
        if (tilemap == null)
        {
            Debug.LogError("BreakableWall script is not attached to a Tilemap!");
        }
    }

    public void BreakTilesInRadius(Vector3 worldPosition, float radius)
    {
        Vector3Int centerTilePosition = tilemap.WorldToCell(worldPosition); // Convert world position to tile coordinates

        // Iterate through all tile positions within the radius
        // Add 1 to the search range to account for the fact that centerTilePosition has been rounded.
        int searchRange = Mathf.CeilToInt(radius) + 1;
        for (int x = -searchRange; x <= searchRange; x++)
        {
            for (int y = -searchRange; y <= searchRange; y++)
            {
                Vector3Int currentTilePosition = new Vector3Int(centerTilePosition.x + x, centerTilePosition.y + y, centerTilePosition.z);
                Vector3 currentTileLocalPosition = tilemap.CellToLocalInterpolated(currentTilePosition + new Vector3(0.5f, 0.5f, 0.5f));
                Vector3 currentTileWorldPosition = tilemap.transform.TransformPoint(currentTileLocalPosition);

                // Check if the current tile is within the smash radius using distance calculation
                if (Vector3.Distance(worldPosition, currentTileWorldPosition) <= radius)
                {
                    if (tilemap.HasTile(currentTilePosition)) // If there's a tile here, remove it
                    {
                        tilemap.SetTile(currentTilePosition, null); // Remove the tile
                        Debug.Log($"Destroyed tile at {currentTilePosition}");
                    }
                }
            }
        }
    }
}
