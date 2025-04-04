using UnityEngine;
using UnityEngine.Events;

public class Key : MonoBehaviour
{
    private Mermaid _followedPlayer;
    [SerializeField]
    private float _followDistance; 

    private UnityAction<MermaidChange> _changedPlayerAction;

    private void Start()
    {
        _changedPlayerAction += OnMermaidsChanged;
        MermaidManager manager = MermaidManager.GetSingleton();
        manager._changedMermaid.AddListener(_changedPlayerAction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mermaid player = collision.GetComponent<Mermaid>();
        MermaidManager manager = MermaidManager.GetSingleton();
        if (player && manager.GetActiveCharacter() == player)
        {
            FollowPlayer(player);
        }
    }

    private void OnMermaidsChanged(MermaidChange change)
    {
        if (change.WasDestroyed(_followedPlayer))
        {
            _followedPlayer = change._active;
        }
    }

    private void FollowPlayer(Mermaid mermaid)
    {
        _followedPlayer = mermaid;
    }

    private void Update()
    {
        if (_followedPlayer)
        {
            Vector3 playerOffset = _followedPlayer.transform.position - transform.position;
            float length = playerOffset.magnitude;
            if (length > _followDistance)
            {
                transform.position += playerOffset * ((length - _followDistance) / length);
            }
        }
    }
}
