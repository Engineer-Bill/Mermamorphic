using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField]
    private Door[] _doors;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Mermaid collidedMermaid = collision.collider.GetComponent<Mermaid>();
        if (collidedMermaid)
        {
            if (collidedMermaid.TryConsumeKey())
            {
                GetComponent<Collider2D>().enabled = false;
                foreach (Door door in _doors)
                {
                    door.OpenDoor();
                }
            }
        }
    }
}
