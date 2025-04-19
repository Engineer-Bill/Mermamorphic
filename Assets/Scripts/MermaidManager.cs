using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public struct MermaidChange
{
    public Mermaid _active;
    public Mermaid[] _created;
    public Mermaid[] _destroyed;

    public MermaidChange(Mermaid active)
    {
        _active = active;
        _created = new Mermaid[0];
        _destroyed = new Mermaid[0];
    }

    public MermaidChange(Mermaid active, List<Mermaid> created, List<Mermaid> destroyed)
    {
        _active = active;
        _created = created.ToArray();
        _destroyed = destroyed.ToArray();
    }

    public bool WasDestroyed(Mermaid mermaid)
    {
        foreach (Mermaid destroyed in _destroyed)
        {
            if (destroyed == mermaid)
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class ChangedMermaidEvent : UnityEvent<MermaidChange> { }

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

    public ChangedMermaidEvent _changedMermaid;

    [SerializeField]
    private AudioClip _mergeSound;

    [SerializeField]
    private AudioClip _splitSound;

    private AudioSource _audioSource;

    private static MermaidManager _singleton;

    private float _nextSwitchTime = 0.0f;

    public static MermaidManager GetSingleton()
    {
        return _singleton;
    }

    public void Awake()
    {
        _singleton = this;
        _nextSwitchTime = Time.time;

        _audioSource = GetComponent<AudioSource>();                 //SFX Junk
        if (_audioSource == null)                                   //SFX Junk
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Start()
    {
        if (_activeMermaid == null)
        {
            _activeMermaid = GetMermaids()[0];
        }
        AlertChanged(new MermaidChange(_activeMermaid));
    }

    public void Update()
    {
    }

    public void OnSwitchCharactersLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time >= _nextSwitchTime)
            {
                SwitchActiveCharacter(-1);
                _nextSwitchTime = Time.time + 0.2f;
            }
        }
    }

    public void OnSwitchCharactersRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time >= _nextSwitchTime)
            {
                SwitchActiveCharacter(1);
                _nextSwitchTime = Time.time + 0.2f;
            }
        }
    }

    private void SwitchActiveCharacter(int offset)
    {
        int currentIndex = 0;
        List<Mermaid> mermaids = GetMermaids();
        for (int i = 0; i < mermaids.Count; i++)
        {
            if (mermaids[i] == _activeMermaid)
            {
                currentIndex = i;
                break;
            }
        }
        currentIndex += offset + mermaids.Count;
        currentIndex = currentIndex % mermaids.Count;
        AlertChanged(new MermaidChange(mermaids[currentIndex]));
    }

    public Mermaid GetActiveCharacter()
    {
        return _activeMermaid;
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
        AlertChanged(new MermaidChange(result, new List<Mermaid> { result }, new List<Mermaid> { mermaid1, mermaid2 }));
        _audioSource.PlayOneShot(_mergeSound); //SFX Junk - merge sound effect plays
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
            _audioSource.PlayOneShot(_splitSound); //SFX Junk
            AlertChanged(new MermaidChange(mermaid1, new List<Mermaid> { mermaid1, mermaid2 }, new List<Mermaid> { mermaid }));
        }
    }

    private void AlertChanged(MermaidChange change)
    {
        _activeMermaid = change._active;
        _camera.target = _activeMermaid.transform;
        _changedMermaid.Invoke(change);
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

    public void OnMove(InputAction.CallbackContext context)
    {
        _activeMermaid.GetComponent<PlayerMovement>().OnMove(context);
    }

    public void OnCombine(InputAction.CallbackContext context)
    {
        _activeMermaid.OnCombine(context);
    }

    public void OnSplit(InputAction.CallbackContext context)
    {
        _activeMermaid.OnSplit(context);
    }
}
