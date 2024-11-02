using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Dependencies")]
    public Sprite[] Sprites;
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D rigidbody2D;
    public WireNetwork Network;

    public float speed = 1;
    public bool shouldOpen = false;

    private float timer = 0;

    private void Update()
    {
        shouldOpen = Network.IsOn;

        timer += Time.deltaTime * speed * (shouldOpen ? 1 : -1);
        timer = Mathf.Clamp(timer, 0, 1);

        var index = (int)(timer * (Sprites.Length - 1));
        SpriteRenderer.sprite = Sprites[index];

        rigidbody2D.simulated = index != Sprites.Length - 1;
    }
}
