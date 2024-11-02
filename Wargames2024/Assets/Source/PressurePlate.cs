using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressurePlate : MonoBehaviour {
    public Door door;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        print("Triggered!");
        door.shouldOpen = true;
        door.shouldChange = true;
    }

    void OnTriggerExit2D(Collider2D col) {
        print("Exit!");
        door.shouldOpen = false;
        door.shouldChange = true;
    }
}
