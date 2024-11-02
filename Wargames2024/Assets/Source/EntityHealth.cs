using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class EntityHealth : MonoBehaviour
{
    public float Health = 100;
    public float MaxHealth = 100;
    public float RegenPerSecond = 0;

    public List<Object> DestroyOnDeath = new();

    private void Update()
    {
        if (Health <= 0)
        {
            foreach (var o in DestroyOnDeath)
            {
                Destroy(o);
            }
        }

        Health += RegenPerSecond * Time.deltaTime;
        Health = Math.Clamp(Health, 0, MaxHealth);
    }
}
