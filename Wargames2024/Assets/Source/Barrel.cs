using UnityEngine;

public class Barrel : MonoBehaviour
{
    [Header("Dependencies")]
    public BarrelAnimation Animation;
    public Rigidbody2D Rigidbody;

    public float SmoothTimeX = 1;
    public float SmoothTimeY = 0.05f;

    public float DistancePerSpriteChange = 1;

    private void Update()
    {
        var velocity = Rigidbody.velocity;
        var localVelocity = Rigidbody.transform.worldToLocalMatrix * velocity;

        // Rigidbody.velocity = Vector3.SmoothDamp(Rigidbody.velocity, targetVelocity, ref referenceVelocity, MovementSmoothTime);
    }
}
