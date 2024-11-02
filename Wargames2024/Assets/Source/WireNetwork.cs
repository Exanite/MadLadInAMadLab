using System;
using System.Collections.Generic;
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
    private float Proportion => Mathf.Clamp01((float)PowerLevel / MaxPowerLevel);

    public bool HasPartialPower => PartialPower != 0;
    public float PartialPower => Invert ? (1 - Proportion) : Proportion;

    public List<WireNetwork> NetworksToActivate = new();

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
    }
}
