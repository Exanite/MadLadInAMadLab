using UnityEngine;

public class Wire : MonoBehaviour
{
    public SpriteRenderer WireOffSpriteRenderer;
    public SpriteRenderer WireOnSpriteRenderer;
    public WireNetwork Network;

    private void Update()
    {
        if (!Network)
        {
            WireOnSpriteRenderer.gameObject.SetActive(false);

            return;
        }

        WireOnSpriteRenderer.gameObject.SetActive(Network.IsOn);
    }
}
