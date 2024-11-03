using System;
using Exanite.Core.Components;
using UnityEngine;
using UnityEngine.UI;

public class GameContext : SingletonBehaviour<GameContext>
{
    [Header("Dependencies")]
    public FireBehaviour FirePrefab;
    public GameLevelOrder GameLevelOrder;
    public Image regenIcon;
    public Image resistIcon;

    [NonSerialized]
    public PlayerCharacter Player;
}
