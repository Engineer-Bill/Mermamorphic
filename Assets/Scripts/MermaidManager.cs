using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class MermaidManager : MonoBehaviour
{
    [System.Serializable]
    public class MermaidEntry
    {
        public Mermaid.Color _color;
        public Mermaid _prefab;
    }

    [SerializeField]
    private List<MermaidEntry> _mermaidPrefabs;

    [SerializeField]
    private Mermaid _activeMermaid;

    [SerializeField]
    private CameraFollow _camera;

    public void Start()
    {
        if (_activeMermaid == null)
        {
            _activeMermaid = GetMermaids()[0];
        }
        SetActiveMermaid(_activeMermaid);
    }

    public void Update()
    {
        // Always reinforce which mermaid is controlled
        foreach (Mermaid child in GetMermaids())
        {
            if (child == _activeMermaid)
            {
                child.GetComponent<PlayerInput>().enabled = true;
            } else
            {
                child.GetComponent<PlayerInput>().enabled = false;
            }
        }
    }

    public void OnSwitchCharactersLeft()
    {
        SwitchActiveCharacter(-1);
    }

    public void OnSwitchCharactersRight()
    {
        SwitchActiveCharacter(1);
    }

    private void SwitchActiveCharacter(int offset)
    {
        int currentIndex = 0;
        List<Mermaid> mermaids = GetMermaids();
        for (int i = 0; i < mermaids.Count; i++) {
            if (mermaids[i] == _activeMermaid)
            {
                currentIndex = i;
                break;
            }
        }
        currentIndex += offset + mermaids.Count;
        currentIndex = currentIndex % mermaids.Count;
        SetActiveMermaid(mermaids[currentIndex]);
    }

    /*
     * All direct children of this object are mermaids.
     */
    public List<Mermaid> GetMermaids()
    {
        var children = new List<Mermaid>();
        foreach (Transform t in transform)
        {
            children.Add(t.GetComponent<Mermaid>());
        }
        return children;
    }

    public Mermaid MergeWith(Mermaid mermaid1, Mermaid mermaid2)
    {
        var color1 = mermaid1.GetColor();
        var color2 = mermaid2.GetColor();

        Mermaid prefab = GetMermaid(color1.Merge(color2));
        if (prefab == null)
        {
            return null;
        }

        Destroy(mermaid1);
        Destroy(mermaid2);
        return Instantiate<Mermaid>(prefab);
    }

    private void SetActiveMermaid(Mermaid mermaid)
    {
        _activeMermaid = mermaid;
        _camera.target = _activeMermaid.transform;
    }
    private Mermaid GetMermaid(Mermaid.Color color)
    {
        foreach (var entry in _mermaidPrefabs)
        {
            if (entry._color == color)
            {
                return entry._prefab;
            }
        }
        return null;
    }
}
