using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    private SceneReference _level;
    [SerializeField]
    private LevelList _levelList;

    [SerializeField] private AudioClip _selectSound; // SFX Junk
    private AudioSource _audioSource;

    private bool _available;

    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        if (_selectSound != null)
            _audioSource.PlayOneShot(_selectSound);
    }

    public void Load()
    {
        _level.Load();
    }
}
