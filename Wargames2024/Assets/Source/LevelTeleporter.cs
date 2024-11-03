using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTeleporter : MonoBehaviour
{
    public Teleporter Teleporter;
    public Collider2D TeleporterCollider;
    public TMP_Text Text;
    public List<GameObject> GameObjectsToDisable;

    public void Initialize(bool isEnabled, int levelIndex, LevelInfo levelInfo)
    {
        if (!isEnabled)
        {
            Teleporter.enabled = false;
            TeleporterCollider.enabled = false;

            foreach (var obj in GameObjectsToDisable)
            {
                obj.SetActive(false);
            }
        }

        Text.text = $"Level {levelIndex}\n{levelInfo.DisplayName}";
    }
}
