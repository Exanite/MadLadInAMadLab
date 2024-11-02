using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    public EntityHealth Health;
    public BurningFragment FragmentPrefab;

    public int MinFragments = 10;
    public int MaxFragments = 30;

    private void OnEnable()
    {
        Health.Died += OnDied;
    }

    private void OnDisable()
    {
        Health.Died -= OnDied;
    }

    private void OnDied()
    {
        var fragmentCount = Random.Range(MinFragments, MaxFragments + 1);
        for (var i = 0; i < fragmentCount; i++)
        {
            var angle = Random.Range(0, 360);

            Instantiate(FragmentPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        }
    }
}