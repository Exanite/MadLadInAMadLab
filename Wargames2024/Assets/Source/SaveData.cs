using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<LevelData> Levels = new();

    [Serializable]
    public class LevelData
    {
        public int Index;
        public string Name;
        public bool IsUnlocked;
    }
}