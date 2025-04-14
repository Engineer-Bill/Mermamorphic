using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    private SceneReference _level;
    [SerializeField]
    private LevelList _levelList;

    private bool _available;

    public void Awake()
    {
    }

    public void Start()
    {
        _available = _levelList.IsAvailable(_level);
        if (!_available)
        {
            Destroy(gameObject);
        }
    }

    public void Selected()
    {
        Invoke("Load", 0.5f);
    }

    public void Load()
    {
        _level.Load();
    }
}
