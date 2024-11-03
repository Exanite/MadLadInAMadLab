using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Project/GameLevelOrder", order = -1000)]
public class GameLevelOrder : ScriptableObject
{
    public List<LevelInfo> Levels = new();

    public (int Index, LevelInfo Info) GetCurrentLevelInfo()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        var currentSceneIndex = Levels.FindIndex(l => l.SceneName == currentSceneName);
        if (currentSceneIndex == -1)
        {
            throw new Exception($"Current scene is not registered in the {typeof(GameLevelOrder).Name}");
        }

        return (currentSceneIndex, Levels[currentSceneIndex]);
    }

    [Serializable]
    public struct LevelInfo
    {
        public string SceneName;
        public string DisplayName;
    }
}
