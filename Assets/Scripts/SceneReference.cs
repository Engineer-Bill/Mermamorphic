using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneReference", menuName = "Mermaids/SceneReference", order = 1)]
public class SceneReference : ScriptableObject
{
    public string _levelName;

    public void Load()
    {
        SceneManager.LoadScene(_levelName);
    }
}
