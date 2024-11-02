using System;
using Exanite.Core.Components;
using UnityEngine;

public class GameContext : SingletonBehaviour<GameContext>
{
    [Header("Dependencies")]
    public FireBehaviour FirePrefab;

    [NonSerialized]
    public PlayerCharacter Player;
}
