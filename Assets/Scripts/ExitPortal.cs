using UnityEngine;

public class ExitPortal : MonoBehaviour
{
    [SerializeField]
    private SceneReference _nextScene;

    private Collider2D _collision;

    public void Start()
    {
        _collision = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _nextScene.Load();
    }
}
