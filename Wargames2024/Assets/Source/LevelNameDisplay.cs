using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNameDisplay : MonoBehaviour
{
    public TMP_Text Text;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateText();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        var currentScene = GameContext.Instance.GameLevelOrder.GetCurrentLevelInfo();
        Text.text = $"Level {currentScene.Index + 1} - {currentScene.Info.DisplayName}";
    }
}
