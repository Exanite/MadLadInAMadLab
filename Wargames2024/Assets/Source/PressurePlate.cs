using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public WireNetwork Network;
    private int count = 0;

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
