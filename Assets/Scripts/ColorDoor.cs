using UnityEngine;
using UnityEngine.Events;

public class ColorDoor : MonoBehaviour
{
    [SerializeField]
    private Mermaid.Color _color;

    private UnityAction<MermaidChange> _changedPlayerAction;
    private Collider2D _collider;

    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();
        //sprite.color = _color.GetColor();

        _collider = GetComponent<Collider2D>();

        _changedPlayerAction += RespondToMerge;

        MermaidManager manager = MermaidManager.GetSingleton();
        manager._changedMermaid.AddListener(_changedPlayerAction);
        Mermaid activeMermaid = manager.GetActiveCharacter();
        if (activeMermaid) {
            OnPlayerChanged(activeMermaid);
        }
    }

    void RespondToMerge(MermaidChange change)
    {
        OnPlayerChanged(change._active);
    }

    void OnPlayerChanged(Mermaid mermaid)
    {
        _collider.enabled = !PassesOtherColor(mermaid.GetColor());
    }

    bool PassesOtherColor(Mermaid.Color other)
    {
        return _color == other;
    }
}
