using System;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public float Health = 100;
    public float MaxHealth = 100;
    public float RegenPerSecond = 0;

    private void Update()
    {
        Health += RegenPerSecond * Time.deltaTime;
        Health = Math.Clamp(Health, 0, MaxHealth);
    }
}
