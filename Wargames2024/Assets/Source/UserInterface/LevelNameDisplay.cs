using System;
using TMPro;
using UnityEngine;

public class LevelNameDisplay : MonoBehaviour
{
    public TMP_Text Text;

    private void Update()
    {
        var currentScene = GameContext.Instance.GameLevelOrder.GetCurrentLevelInfo();
        Text.text =
            $"Level: {TimeSpan.FromSeconds(GameContext.Instance.LevelTimer):g} {(GameContext.Instance.IsLevelTimerPaused ? "(Paused)" : "")}" +
            $"\nTotal: {TimeSpan.FromSeconds(GameContext.Instance.GameTimer):g} {(GameContext.Instance.IsGameTimerPaused ? "(Paused)" : "")}" +
            $"\nLevel {currentScene.Index} - {currentScene.Info.DisplayName}";
    }
}
