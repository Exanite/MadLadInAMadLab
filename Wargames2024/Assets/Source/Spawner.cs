using System;
using Exanite.Core.Pooling;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Dependencies")]
    public Sprite[] Sprites;
    public SpriteRenderer SpriteRenderer;

    [Header("Settings")]
    public GameObject Prefab;
    public WireNetwork Network;
    public float SpawnCheckRadius = 0.75f;

    [Header("Animation")]
    public int animationSpeed = 5;
    private float i = 0;

    private void OnEnable()
    {
        Network.Activated += OnActivated;
    }

    private void OnDisable()
    {
        Network.Activated -= OnActivated;
    }

    private void Update()
    {
        SpriteRenderer.sprite = Sprites[(int)Math.Floor(i) % 6 + 2];
        i += Time.deltaTime * animationSpeed;
    }

    private void OnActivated()
    {
        TrySpawn();
    }

    private void TrySpawn()
    {
        if (Prefab == null)
        {
            return;
        }


        using var _ = ListPool<Collider2D>.Acquire(out var results);
        Physics2D.OverlapCircle(transform.position, SpawnCheckRadius, default, results);
        foreach (var result in results)
        {
            if (result.gameObject != gameObject)
            {
                return;
            }
        }

        Instantiate(Prefab, transform.position, Quaternion.identity);
    }
}
