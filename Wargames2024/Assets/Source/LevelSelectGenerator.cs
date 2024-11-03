using UnityEngine;
using UnityEngine.Serialization;

public class LevelSelectGenerator : MonoBehaviour
{
    public GameObject EndWall;
    [FormerlySerializedAs("Teleporter")]
    public LevelTeleporter TeleporterPrefab;
    public Transform FirstTeleporterPosition;

    public int EndWallOffset = 10;
    public int SpaceBetweenTeleporters = 10;

    private void Start()
    {
        var levels = GameContext.Instance.GameLevelOrder.Levels;

        // Skip 0 because 0 is level select
        for (var i = 1; i < levels.Count; i++)
        {
            var position = FirstTeleporterPosition.transform.position + i * SpaceBetweenTeleporters * Vector3.right;
            var level = levels[i];

            var teleporter = Instantiate(TeleporterPrefab, position, Quaternion.identity);
            teleporter.Initialize(true, i, level);
        }

        var endWallPosition = EndWall.transform.position;
    }
}
