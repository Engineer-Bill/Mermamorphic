using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private float _followDistance; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mermaid player = collision.GetComponent<Mermaid>();
        MermaidManager manager = MermaidManager.GetSingleton();
        if (player && manager.GetActiveCharacter() == player)
        {
            Mermaid heldBy = HeldBy();
            if (heldBy)
            {
                heldBy.DropKey();
            }
            player.PickupKey(this);
        }
    }

    public Mermaid HeldBy()
    {
        foreach (Mermaid other in MermaidManager.GetSingleton().GetMermaids())
        {
            if (other.HeldKey() == this)
            {
                return other;
            }
        }
        return null;
    }

    public void Follow(Mermaid followedPlayer)
    {
        Vector3 playerOffset = followedPlayer.transform.position - transform.position;
        float length = playerOffset.magnitude;
        if (length > _followDistance)
        {
            transform.position += playerOffset * ((length - _followDistance) / length);
        }
    }
}
