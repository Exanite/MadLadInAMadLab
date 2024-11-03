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

    private int RawMaxPowerLevel => Threshold;
    private int RawPowerLevel => Mathf.Clamp(EnergySources.Count, 0, RawMaxPowerLevel);
    private float RawProportion
    {
        get
        {
            if (RawMaxPowerLevel == 0)
            {
                return 1;
            }

            return Mathf.Clamp01((float)RawPowerLevel / RawMaxPowerLevel);
        }
    }

    public bool HasPartialPower => PartialPower != 0;
    public float PartialPower => Invert ? 1 - RawProportion : RawProportion;

    public List<WireNetwork> NetworksToActivate = new();

    private bool wasFullyOn;
    public bool IsFullyOn => Mathf.Approximately(PartialPower, 1);

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
