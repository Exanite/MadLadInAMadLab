using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNameDisplay : MonoBehaviour
{
    public TMP_Text Text;

    private string sceneText = "";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateSceneText();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        UpdateSceneText();
    }

    private void UpdateSceneText()
    {
        var currentScene = GameContext.Instance.GameLevelOrder.GetCurrentLevelInfo();
        sceneText = $"\nLevel {currentScene.Index} - {currentScene.Info.DisplayName}";
    }

    private void Update()
    {
        var currentScene = GameContext.Instance.GameLevelOrder.GetCurrentLevelInfo();
        Text.text =
            $"Level: {TimeSpan.FromSeconds(GameContext.Instance.LevelTimer):g} {(GameContext.Instance.IsLevelTimerPaused ? "(Paused)" : "")}" +
            $"\nTotal: {TimeSpan.FromSeconds(GameContext.Instance.GameTimer):g} {(GameContext.Instance.IsGameTimerPaused ? "(Paused)" : "")}" +
            $"{sceneText}";
    }
}
