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

    public event Action Died;

    private void Update()
    {
        if (Health <= 0)
        {
            Died?.Invoke();

            foreach (var o in DestroyOnDeath)
            {
                Destroy(o);
            }

            return;
        }

        Health += RegenPerSecond * Time.deltaTime;
        Health = Math.Clamp(Health, 0, MaxHealth);
    }
}
