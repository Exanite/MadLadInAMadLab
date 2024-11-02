using Exanite.Core.Utilities;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [Header("Dependencies")]
    public BarrelAnimation Animation;
    public Rigidbody2D Rigidbody;
    public SpriteRenderer SpriteRenderer;

    public float SmoothTimeY = 0.05f;

    public float DistancePerSpriteChange = 1;

    private float referenceVelocityY;

    private float distanceTraveledX = 0;

    private void Update()
    {
        var currentVelocity = Rigidbody.velocity;
        var currentLocalVelocity = Rigidbody.transform.worldToLocalMatrix * currentVelocity;

        // Rolling animation
        distanceTraveledX += currentLocalVelocity.x * Time.deltaTime;
        var spriteIndex = (int)MathUtility.Wrap(distanceTraveledX / DistancePerSpriteChange, 0, Animation.Sprites.Length);
        SpriteRenderer.sprite = Animation.Sprites[spriteIndex];

        // Y velocity damping
        var velocity = Rigidbody.transform.worldToLocalMatrix * Rigidbody.velocity;
        velocity.y = Mathf.SmoothDamp(velocity.y, 0, ref referenceVelocityY, SmoothTimeY);
        Rigidbody.velocity = Rigidbody.transform.localToWorldMatrix * velocity;
    }
}
