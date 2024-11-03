using System;
using System.Collections.Generic;
using Exanite.Core.Utilities;
using UnityEngine;

public class WireNetwork : MonoBehaviour
{
    [NonSerialized]
    public HashSet<GameObject> EnergySources = new();

    [Header("Settings")]
    public int Threshold = 1;
    public bool Invert = false;

    private int PowerLevel => EnergySources.Count;
    private int MaxPowerLevel => Threshold;
    private float Proportion
    {
        get
        {
            if (MaxPowerLevel == 0)
            {
                return 0;
            }

            return Mathf.Clamp01((float)PowerLevel / MaxPowerLevel);
        }
    }

    public bool HasPartialPower => PartialPower != 0;
    public float PartialPower
    {
        get
        {
            if (MaxPowerLevel == 0)
            {
                return 1;
            }

            return Invert ? (1 - Proportion) : Proportion;
        }
    }

    public List<WireNetwork> NetworksToActivate = new();

    private bool wasFullyOn;
    public bool IsFullyOn
    {
        get
        {
            var result = EnergySources.Count >= Threshold;
            if (Invert)
            {
                result = !result;
            }

            return result;
        }
    }

    public event Action Activated;

    private void Update()
    {
        if (IsFullyOn)
        {
            foreach (var network in NetworksToActivate)
            {
                network.EnergySources.Add(gameObject);
            }
        }
        else
        {
            foreach (var network in NetworksToActivate)
            {
                network.EnergySources.Remove(gameObject);
            }
        }

        if (wasFullyOn != IsFullyOn && IsFullyOn)
        {
            "Network activated".Dump();
            Activated?.Invoke();
        }

        wasFullyOn = IsFullyOn;
    }
}
