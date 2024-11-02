using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressurePlate : MonoBehaviour {
    public Door door;
    // Start is called before the first frame update
    int count = 0;
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        print("Triggered!");
        door.shouldOpen = true;
        door.shouldChange = true;
        count++;
    }

    void OnTriggerExit2D(Collider2D col) {
        print("Exit!");
        count--;
        if (count == 0) {
            door.shouldOpen = false;
            door.shouldChange = true;
        }
    }
}
