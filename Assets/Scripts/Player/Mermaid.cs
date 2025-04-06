using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class Mermaid : MonoBehaviour
{
    public enum Color { None, Red, Yellow, Blue, Orange, Green, Purple};

    [SerializeField]
    private Color _color;

    [SerializeField]
    private Collider2D _mergeCheckCollider;

    [SerializeField]
    private Key _pickedUpKey;

    private UnityAction<MermaidChange> _changedPlayerAction;

    [SerializeField]
    private Animator _animator;

    static ContactFilter2D _filter = new ContactFilter2D();

    public Color GetColor()
    {
        return _color;
    }

    public Key HeldKey()
    {
        return _pickedUpKey;
    }

    public bool PickupKey(Key key)
    {
        if (!_pickedUpKey)
        {
            _pickedUpKey = key;
            return true;
        }
        return false;
    }

    public bool TryConsumeKey()
    {
        bool result = _pickedUpKey;
        if (result)
        {
            Destroy(_pickedUpKey.gameObject);
        }
        return result;
    }

    private void Start()
    {
        _changedPlayerAction += OnMermaidsChanged;
        MermaidManager manager = MermaidManager.GetSingleton();
        manager._changedMermaid.AddListener(_changedPlayerAction);
    }


    private void Update()
    {
        if (_pickedUpKey)
        {
            _pickedUpKey.Follow(this);
        }
    }

    private void OnMermaidsChanged(MermaidChange change)
    {
        if (change.WasDestroyed(this) && _pickedUpKey)
        {
            if (change._active.PickupKey(_pickedUpKey))
            {
                _pickedUpKey = null;
            }
        }
    }


    public void OnCombine(CallbackContext context)
    {
        if (!context.performed)
        {
            // Early return if the button wasn't just pressed.
            return;
        }
        var colliders = new List<Collider2D>();

        _filter.NoFilter();
        _mergeCheckCollider.Overlap(_filter, colliders);

        Mermaid bestMerge = null;
        float bestMergeDistanceSq = float.MaxValue;

        foreach (var collider in colliders)
        {
            if (collider.attachedRigidbody == null)
            {
                continue;
            }
            Mermaid other = collider.attachedRigidbody.GetComponent<Mermaid>();
            if (other == null)
            {
                continue;
            }
            float distanceSq = (other.transform.position - transform.position).sqrMagnitude;

            if (distanceSq < bestMergeDistanceSq)
            {
                bestMerge = other;
            }
        }

        var manager = GetComponentInParent<MermaidManager>();
        if (bestMerge != null && manager != null)
        {
            manager.Merge(this, bestMerge);
        }
    }

    public void OnSplit(CallbackContext context)
    {
        if (!context.performed)
        {
            // Early return if the button wasn't just pressed.
            return;
        }

        var manager = GetComponentInParent<MermaidManager>();
        if (manager != null)
        {
            manager.Split(this);
        }
    }
}

static class ColorExtensions
{
    public static bool IsPrimary(this Mermaid.Color self)
    {
        return (self == Mermaid.Color.Red || self == Mermaid.Color.Blue || self == Mermaid.Color.Yellow);
    }

    public static bool IsSecondary(this Mermaid.Color self)
    {
        return (self == Mermaid.Color.Orange || self == Mermaid.Color.Green || self == Mermaid.Color.Purple);
    }

    public static Mermaid.Color Merge(this Mermaid.Color self, Mermaid.Color other)
    {
        // Only primary colors can merge
        if (!other.IsPrimary())
        {
            return Mermaid.Color.None;
        }
        // You can't merge with yourself
        if (other == self)
        {
            return Mermaid.Color.None;
        }

        if (self == Mermaid.Color.Red)
        {
            if (other == Mermaid.Color.Blue)
            {
                return Mermaid.Color.Purple;
            } else if (other == Mermaid.Color.Yellow)
            {
                return Mermaid.Color.Orange;
            }
        } else if (self == Mermaid.Color.Blue)
        {
            if (other == Mermaid.Color.Red)
            {
                return Mermaid.Color.Purple;
            } else if (other == Mermaid.Color.Yellow)
            {
                return Mermaid.Color.Green;
            }
        } else if (self == Mermaid.Color.Yellow)
        {
            if (other == Mermaid.Color.Blue)
            {
                return Mermaid.Color.Green;
            } else if (other == Mermaid.Color.Red)
            {
                return Mermaid.Color.Orange;
            }
        }
        return Mermaid.Color.None;
    }

    public static (Mermaid.Color, Mermaid.Color) Split(this Mermaid.Color self)
    {
        if (!self.IsSecondary())
        {
            return (Mermaid.Color.None, Mermaid.Color.None);
        }

        if (self == Mermaid.Color.Orange)
        {
            return (Mermaid.Color.Red, Mermaid.Color.Yellow);
        } else if (self == Mermaid.Color.Green)
        {
            return (Mermaid.Color.Yellow, Mermaid.Color.Blue);
        } else if (self == Mermaid.Color.Purple)
        {
            return (Mermaid.Color.Red, Mermaid.Color.Blue);
        } else
        {
            return (Mermaid.Color.None, Mermaid.Color.None);
        }
    }

    public static Color GetColor(this Mermaid.Color color)
    {
        switch (color) {
            case Mermaid.Color.Red:
                return Color.red;
            case Mermaid.Color.Yellow:
                return Color.yellow;
            case Mermaid.Color.Blue:
                return Color.blue;
            case Mermaid.Color.Orange:
                return new Color(1.0f, 0.5f, 0);
            case Mermaid.Color.Green:
                return Color.green;
            case Mermaid.Color.Purple:
                return new Color(0.5f, 0, 1.0f);
            default:
                return Color.white;
        }
    }
};
