using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public SpriteRenderer Sprite1;
    public SpriteRenderer Sprite2;
    public SpriteRenderer Sprite3;
    public GameObject Audio;

    public WireNetwork Network;
    private HashSet<Collider2D> colliders = new();

    private void Update()
    {
        colliders.RemoveWhere(x => x == null);

        Sprite3.gameObject.SetActive(colliders.Count > 0);
        Audio.gameObject.SetActive(colliders.Count > 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        colliders.Add(col);
        Network.EnergySources.Add(gameObject);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        colliders.Remove(col);
        if (colliders.Count == 0)
        {
            Network.EnergySources.Remove(gameObject);
        }
    }
}
