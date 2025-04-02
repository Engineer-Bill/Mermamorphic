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

    public Mermaid Merge(Mermaid mermaid1, Mermaid mermaid2)
    {
        if (mermaid1 != _activeMermaid)
        {
            return null;
        }

        var color1 = mermaid1.GetColor();
        var color2 = mermaid2.GetColor();
        var mergeColor = color1.Merge(color2);

        Mermaid prefab = GetMermaid(mergeColor);
        if (prefab == null)
        {
            return null;
        }

        Vector3 newMermaidPosition = mermaid1.transform.position;
        Destroy(mermaid1.gameObject);
        Destroy(mermaid2.gameObject);
        var result = Instantiate<Mermaid>(prefab, transform);
        result.transform.position = newMermaidPosition;
        SetActiveMermaid(result);
        return result;
    }

    public void Split(Mermaid mermaid)
    {
        if (mermaid != _activeMermaid)
        {
            return;
        }
        (Mermaid.Color color1, Mermaid.Color color2) = mermaid.GetColor().Split();
        Mermaid mermaidPrefab1 = GetMermaid(color1);
        Mermaid mermaidPrefab2 = GetMermaid(color2);
        if (mermaidPrefab1 != null && mermaidPrefab2 != null)
        {
            Vector3 newMermaidPosition = mermaid.transform.position;
            Destroy(mermaid.gameObject);
            var mermaid1 = Instantiate<Mermaid>(mermaidPrefab1, transform);
            var mermaid2 = Instantiate<Mermaid>(mermaidPrefab2, transform);
            mermaid1.transform.position = newMermaidPosition;
            mermaid2.transform.position = newMermaidPosition;
            SetActiveMermaid(mermaid1);
        }
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
