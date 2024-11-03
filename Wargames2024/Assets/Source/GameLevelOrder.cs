using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Project/GameLevelOrder", order = -1000)]
public class GameLevelOrder : ScriptableObject
{
    public List<LevelInfo> Levels = new();

    [Serializable]
    public struct LevelInfo
    {
        public string SceneName;
        public string DisplayName;
    }
}
