using UnityEngine;

public class Wire : MonoBehaviour
{
    private static readonly int PostMultiplier = Shader.PropertyToID("_PostMultiplier");

    public WireNetwork Network;

    public SpriteRenderer Sprite1;
    public SpriteRenderer Sprite2;
    public SpriteRenderer Sprite3;

    private Material material2;

    private void Start()
    {
        material2 = Sprite2.material;
    }

    private void Update()
    {
        if (!Network)
        {
            Sprite2.gameObject.SetActive(false);
            Sprite3.gameObject.SetActive(false);

            return;
        }

        Sprite2.gameObject.SetActive(Network.HasPartialPower);
        Sprite3.gameObject.SetActive(Network.IsFullyOn);

        material2.SetFloat(PostMultiplier, Network.PartialPower);
    }

    private void OnDestroy()
    {
        Destroy(material2);
    }
}
