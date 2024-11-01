using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    // Fire can spread, ignite, and burn
    // Spread: Creates a new copy of the fire in a random location around the fire
    // Ignite: Sets things the fire touches on fire, if it is not already on fire
    // Burn: Damages things that are nearby, if they are burnable

    public FireBehaviour FirePrefab;

    public float DamagePerSecond = 10;
    public float Duration = 10;

    [Space]
    public float MinSpreadRange = 2;
    public float MaxSpreadRange = 4;

    [Space]
    public int SpreadTryCount = 3;
    public float SpreadProbability = 0.5f;
    public float SpreadCheckRadius = 0.5f;

    [Space]
    public float MinSpreadTime = 3;
    public float MaxSpreadTime = 9;

    private float spreadTimer;
    private float nextSpreadTime;

    private void Start()
    {
        UpdateSpreadTime();
    }

    private void Update()
    {
        spreadTimer += Time.deltaTime;
        if (spreadTimer > nextSpreadTime)
        {
            TrySpread();
            UpdateSpreadTime();
        }
    }

    private void TrySpread()
    {
        for (var i = 0; i < SpreadTryCount; i++)
        {
            var spreadRange = Random.Range(MinSpreadRange, MaxSpreadRange);
            var spreadPosition = Random.insideUnitCircle.normalized * spreadRange;

            if (Physics2D.OverlapCircle(spreadPosition, SpreadCheckRadius))
            {
                continue;
            }

            Instantiate(FirePrefab, spreadPosition, Quaternion.identity);
        }
    }

    private void UpdateSpreadTime()
    {
        nextSpreadTime = Random.Range(MinSpreadTime, MaxSpreadTime);
    }
}