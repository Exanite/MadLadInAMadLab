using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AcidBarrel : MonoBehaviour {
    public Rigidbody2D Rigidbody;
    private Vector2 prevVelocity;
    public float threshold = 0.01f;
    void Update() {
        if (Math.Abs((prevVelocity - Rigidbody.velocity).magnitude) * Time.deltaTime > threshold) {
            Instantiate(GameContext.Instance.AcidPrefab, transform.position, Quaternion.identity);
        }
        prevVelocity = Rigidbody.velocity;
    }
}
