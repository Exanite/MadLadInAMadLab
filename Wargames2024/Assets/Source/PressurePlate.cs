using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public SpriteRenderer Sprite1;
    public SpriteRenderer Sprite2;
    public SpriteRenderer Sprite3;
    public GameObject Audio;

    public WireNetwork Network;
    private int count = 0;

    private void Update()
    {
        Sprite3.gameObject.SetActive(count > 0);
        Audio.gameObject.SetActive(count > 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        count++;

        Network.EnergySources.Add(gameObject);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        count--;

        if (count == 0)
        {
            Network.EnergySources.Remove(gameObject);
        }
    }
}
