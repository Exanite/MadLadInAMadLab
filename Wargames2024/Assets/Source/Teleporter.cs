using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [Header("Dependencies")]
    public Sprite[] Sprites;
    public SpriteRenderer SpriteRenderer;

    [Header("Scene settings")]
    public TeleporterType Type = TeleporterType.GoToNextScene;
    public string scene = "";

    [Header("Animation")]
    public int animationSpeed = 5;
    private float i = 0;

    private void Update()
    {
        SpriteRenderer.sprite = Sprites[(int)Math.Floor(i) % 6 + 2];
        i += Time.deltaTime * animationSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.attachedRigidbody && col.attachedRigidbody.TryGetComponent(out PlayerCharacter _))
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        if (Type == TeleporterType.GoToSpecifiedScene)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);

            return;
        }

        var levelOrder = GameContext.Instance.GameLevelOrder;

        var currentSceneName = SceneManager.GetActiveScene().name;
        var currentSceneIndex = levelOrder.Levels.FindIndex(l => l.SceneName == currentSceneName);
        if (currentSceneIndex == -1)
        {
            Debug.LogError($"Current scene is not registered in the {typeof(GameLevelOrder).Name}");

            return;
        }

        var nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex >= levelOrder.Levels.Count)
        {
            Debug.LogWarning("No next scene");

            return;
        }

        SceneManager.LoadScene(levelOrder.Levels[nextSceneIndex].SceneName, LoadSceneMode.Single);
    }
}
