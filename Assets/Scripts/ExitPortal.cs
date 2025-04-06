using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{
    [SerializeField]
    private SceneReference _nextScene;

    [SerializeField]
    private LevelList _levelList;

    [SerializeField]
    private TextMesh _countText;

    [SerializeField]
    private int _requiredMermaidsToLeave;

    private Collider2D _collision;

    public void Awake()
    {
        Assert.IsNotNull(_nextScene);
        Assert.IsNotNull(_levelList);
        Assert.IsNotNull(_countText);
    }

    public void Start()
    {
        _collision = GetComponent<Collider2D>();
        if (_requiredMermaidsToLeave == 0)
        {
            _requiredMermaidsToLeave = GetTotalMermaidCount();
        }
    }

    public void Update()
    {
        int exiting = GetExitingMermaidCount();

        _countText.text = exiting.ToString() + " / " + _requiredMermaidsToLeave.ToString();
        
        if (exiting >= _requiredMermaidsToLeave)
        {
            Exit();
        }
    }

    private int GetExitingMermaidCount()
    {
        int result = 0;
        var colliders = new List<Collider2D>();
        _collision.Overlap(colliders);
        foreach (Collider2D otherCollider in colliders)
        {
            var mermaid = otherCollider.GetComponent<Mermaid>();
            if (mermaid)
            {
                result += mermaid.GetColor().MermaidCount();
            }
        }
        return result;
    }

    private int GetTotalMermaidCount()
    {
        int result = 0;
        foreach (Mermaid mermaid in MermaidManager.GetSingleton().GetMermaids())
        {
            result += mermaid.GetColor().MermaidCount();
        }
        return result;
    }

    private void Exit()
    {
        _levelList.MakeAvailable(_nextScene);
    }
}
