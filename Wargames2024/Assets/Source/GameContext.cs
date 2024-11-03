using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Exanite.Core.Components;
using Exanite.Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameContext : SingletonBehaviour<GameContext>
{
    [Header("Dependencies")]
    public FireBehaviour FirePrefab;
    public AcidBehavior AcidPrefab;
    public GameLevelOrder GameLevelOrder;

    [NonSerialized] public PlayerCharacter Player;

    [NonSerialized] public SaveData SaveData;

    [NonSerialized] public float GameTimer;
    [NonSerialized] public bool IsGameTimerPaused;

    [NonSerialized] public float LevelTimer;
    [NonSerialized] public bool IsLevelTimerPaused;

    public List<string> GameTimerStartScenes = new List<string>() { "Level1" };
    public List<string> GameTimerPauseScenes = new List<string>() { "LevelDev", "LevelSelect" };

    protected override void OnEnable()
    {
        base.OnEnable();

        Load();
        OnLevelLoaded();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (!IsGameTimerPaused)
        {
            GameTimer += Time.deltaTime;
        }

        if (!IsLevelTimerPaused)
        {
            LevelTimer += Time.deltaTime;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnLevelLoaded();
    }

    public void OnLevelLoaded()
    {
        var currentLevel = GameLevelOrder.GetCurrentLevelInfo();

        // Level timer
        LevelTimer = 0;
        IsLevelTimerPaused = false;

        // Game timer
        if (GameTimerStartScenes.Contains(currentLevel.Info.SceneName))
        {
            GameTimer = 0;
            IsGameTimerPaused = false;
        }

        if (GameTimerPauseScenes.Contains(currentLevel.Info.SceneName))
        {
            IsGameTimerPaused = true;
        }

        // Unlock current level
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

        try
        {
            var json = new FileInfo(Path.Join(Application.persistentDataPath, "save.json")).Open(FileMode.Open, FileAccess.Read).ReadAsStringAndDispose();
            var loadedData = JsonUtility.FromJson<SaveData>(json);
            MigrateData(loadedData, SaveData);
        }
        catch
        {
            // Do nothing
        }

        JsonUtility.ToJson(SaveData).Dump("Loaded save data");
    }

    public void Save()
    {
        var newSave = GenerateNewSaveData();
        MigrateData(SaveData, newSave);

        SaveData = newSave;

        var saveFile = new FileInfo(Path.Join(Application.persistentDataPath, "save.json"));
        saveFile.Delete();

        var json = JsonUtility.ToJson(newSave);

        using var fileStream = saveFile.Open(FileMode.CreateNew, FileAccess.Write);
        using var streamWriter = new StreamWriter(fileStream);
        streamWriter.Write(json);

        json.Dump("Saved the game");
    }

    private void MigrateData(SaveData from, SaveData to)
    {
        var unlockedLevels = new HashSet<int>();
        foreach (var level in from.Levels)
        {
            if (level.IsUnlocked)
            {
                unlockedLevels.Add(level.Index);
            }
        }

        foreach (var unlockedLevelIndex in unlockedLevels)
        {
            var level = to.Levels.FirstOrDefault(l => l.Index == unlockedLevelIndex);
            if (level != null)
            {
                level.IsUnlocked = true;
            }
        }
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
