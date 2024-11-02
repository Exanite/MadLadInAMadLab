using UnityEngine;
using Exanite.Core.Utilities;
using UnityEngine.InputSystem.Controls;
using System;


public class Door : MonoBehaviour{
    [Header("Dependencies")]
    public Sprite[] Sprites;
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D rigidbody2D;
    // Update is called once per frame
    float i = 0;
    bool open = false;
    public bool shouldOpen = false;
    public bool shouldChange = false;
    void Update() {
        float delta = Time.deltaTime;
        SpriteRenderer.sprite = Sprites[(int) Math.Floor(i)];
        if (shouldChange) {
            if (shouldOpen) {
                i += delta * 3;
                if (i >= Sprites.Length-1) {
                    open = true;
                    shouldChange = false;
                    i = Sprites.Length-1;
                }
            } else {
                i -= delta * 3;
                if (i <= 0) {
                    open = false;
                    shouldOpen = false;
                    i = 0;
                }
            }
            SpriteRenderer.sprite = Sprites[(int) Math.Floor(i)];
            rigidbody2D.simulated = !open || !shouldOpen;
        }
    }
}
