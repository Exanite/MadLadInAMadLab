using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {
    [Header("Potion Settings")]
    public string PotionType;
    public int level;
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.attachedRigidbody && col.attachedRigidbody.TryGetComponent(out PlayerCharacter player)){
            if (PotionType == "Regeneration" && player.statusEffects[0][1] < 20 * level) {
                player.statusEffects[0][0] = 3;
                player.statusEffects[0][1] = 20 * level;
            }
            if (PotionType == "Fire Resistance" && player.statusEffects[1][0] < 5 * level) {
                player.statusEffects[1][0] = 5 * level;
            }
            Destroy(gameObject);
        }
    }
}
