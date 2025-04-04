using UnityEngine;
using UnityEngine.Events;

public class ColorDoor : MonoBehaviour
{
    [SerializeField]
    private Mermaid.Color _color;

    private UnityAction<Mermaid> _changedPlayerAction;
    private Collider2D _collider;

    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();
        sprite.color = _color.GetColor();

        _collider = GetComponent<Collider2D>();

        _changedPlayerAction += OnPlayerChanged;

        var managers = FindObjectsByType<MermaidManager>(FindObjectsSortMode.None);
        managers[0]._changedMermaid.AddListener(_changedPlayerAction);
        Mermaid activeMermaid = managers[0].GetActiveCharacter();
        if (activeMermaid) {
            OnPlayerChanged(activeMermaid);
        }
    }

    void OnPlayerChanged(Mermaid newActiveMermaid)
    {
        _collider.enabled = !PassesOtherColor(newActiveMermaid.GetColor());
    }

    bool PassesOtherColor(Mermaid.Color other)
    {
        return _color == other;
    }
}
