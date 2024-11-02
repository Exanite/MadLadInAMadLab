using UnityEngine;

public class BurningFragment : MonoBehaviour
{
    public Rigidbody2D Rigidbody;

    public float InitialSpeed = 50;
    public float Duration = 1;

    private float lifetime;

    private void Start()
    {
        Rigidbody.velocity = transform.right * InitialSpeed;
    }

    private void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > Duration)
        {
            Destroy(gameObject);
        }
    }
}