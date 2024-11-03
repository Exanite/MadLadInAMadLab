using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AcidBarrel : MonoBehaviour {
    public Rigidbody2D Rigidbody;
    private Vector2 prevVelocity;
    void Update() {
        print("velocity: " + Math.Abs((prevVelocity - Rigidbody.velocity).magnitude * Time.deltaTime));

        if (Math.Abs((prevVelocity - Rigidbody.velocity).magnitude) * Time.deltaTime > 0.01) {
            Instantiate(GameContext.Instance.AcidPrefab, transform.position, Quaternion.identity);
        }
        prevVelocity = Rigidbody.velocity;
    }
}
