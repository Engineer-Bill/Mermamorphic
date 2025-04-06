using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "Mermaids/LevelList", order = 2)]
public class LevelList : ScriptableObject
{
    [Serializable]
    private class LevelInfo
    {
        public SceneReference _level;
        public bool _available;
    }

    [SerializeField]
    private LevelInfo[] levels;
    [SerializeField]
    private SceneReference _levelSelect;

    public void MakeAvailable(SceneReference scene)
    {
        foreach (LevelInfo levelInfo in levels)
        {
            if (levelInfo._level == scene)
            {
                levelInfo._available = true;
            }
        }
        _levelSelect.Load();
    }

    public bool IsAvailable(SceneReference scene)
    {
        foreach (LevelInfo levelInfo in levels)
        {
            if (levelInfo._level == scene)
            {
                return levelInfo._available;
            }
        }
        return false;
    }
}
