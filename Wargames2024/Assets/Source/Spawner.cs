using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Dependencies")]
    public Sprite[] Sprites;
    public SpriteRenderer SpriteRenderer;

    [Header("Settings")]
    public GameObject Prefab;
    public WireNetwork Network;

    [Header("Animation")]
    public int animationSpeed = 5;
    private float i = 0;

    private void Update()
    {
        SpriteRenderer.sprite = Sprites[(int)Math.Floor(i) % 6 + 2];
        i += Time.deltaTime * animationSpeed;
    }
}
