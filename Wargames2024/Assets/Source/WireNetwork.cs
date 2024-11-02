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

    public bool IsOn
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
}
