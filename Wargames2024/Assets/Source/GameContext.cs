using System;
using System.Collections.Generic;
using System.Linq;
using Exanite.Core.Components;
using Exanite.Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameContext : SingletonBehaviour<GameContext>
{
    [Header("Dependencies")]
    public FireBehaviour FirePrefab;
    public GameLevelOrder GameLevelOrder;

    [NonSerialized]
    public PlayerCharacter Player;

    [NonSerialized]
    public SaveData SaveData;

    protected override void OnEnable()
    {
        base.OnEnable();

        Load();
        UnlockCurrentLevel();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        UnlockCurrentLevel();
    }

    public void UnlockCurrentLevel()
    {
        var currentLevel = GameLevelOrder.GetCurrentLevelInfo();
        var level = SaveData.Levels.FirstOrDefault(l => l.Index == currentLevel.Index);
        if (level != null)
        {
            level.IsUnlocked = true;
        }

        Save();
    }

    public void Load()
    {
        SaveData = GenerateNewSaveData();

        JsonUtility.ToJson(SaveData).Dump("Loaded save data");
    }

    public void Save()
    {
        var unlockedLevels = new HashSet<int>();
        foreach (var level in SaveData.Levels)
        {
            if (level.IsUnlocked)
            {
                unlockedLevels.Add(level.Index);
            }
        }

        var newSave = GenerateNewSaveData();
        foreach (var unlockedLevelIndex in unlockedLevels)
        {
            var level = newSave.Levels.FirstOrDefault(l => l.Index == unlockedLevelIndex);
            if (level != null)
            {
                level.IsUnlocked = true;
            }
        }

        SaveData = newSave;

        JsonUtility.ToJson(newSave).Dump("Saved the game");
    }

    private SaveData GenerateNewSaveData()
    {
        var result = new SaveData();
        for (var i = 0; i < GameLevelOrder.Levels.Count; i++)
        {
            var level = new SaveData.LevelData()
            {
                Index = i,
                Name = GameLevelOrder.Levels[i].DisplayName,
                IsUnlocked = false,
            };

            if (i == 0 || i == 1)
            {
                level.IsUnlocked = true;
            }

            result.Levels.Add(level);
        }

        return result;
    }
}
