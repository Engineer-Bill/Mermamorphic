using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private float _scrollSpeed = 0.1f;

    void Update()
    {
        if (_scrollRect.verticalNormalizedPosition > 0)
        {
            _scrollRect.verticalNormalizedPosition -= _scrollSpeed * Time.deltaTime;
        }
    }
}
