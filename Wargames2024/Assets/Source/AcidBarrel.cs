using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AcidBarrel : MonoBehaviour {
    public Rigidbody2D Rigidbody;
    private Vector2 prevVelocity;
    public float threshold = 500f;
    void FixedUpdate() {
        if ((prevVelocity - Rigidbody.velocity).magnitude > threshold) {
            Instantiate(GameContext.Instance.AcidPrefab, transform.position, Quaternion.identity);
        }
        prevVelocity = Rigidbody.velocity;
    }
}
