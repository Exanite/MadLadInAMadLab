using System.Collections;
using Exanite.Core.Pooling;
using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    public EntityHealth Health;
    public BurningFragment FragmentPrefab;

    public int MinFragments = 10;
    public int MaxFragments = 30;

    public float ExplosionDamage = 25;
    public float ExplosionRadius = 10;
    public float ExplosionForce = 250;
    public float ExplosionDuration = 0.3f;
    public float TimeBetweenExplosionTicks = 0.05f;
    public AnimationCurve ExplosionFalloff = new();

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
        var lifetime = new GameObject("Explosion");
        StartCoroutine(Explode(lifetime));
    }

    private IEnumerator Explode(GameObject lifetime)
    {
        var position = transform.position;

        var fragmentCount = Random.Range(MinFragments, MaxFragments + 1);
        for (var i = 0; i < fragmentCount; i++)
        {
            var angle = Random.Range(0, 360);

            Instantiate(FragmentPrefab, position, Quaternion.Euler(0, 0, angle));
        }

        using var _ = ListPool<Collider2D>.Acquire(out var results);
        Physics2D.OverlapCircle(position, ExplosionRadius, default, results);

        foreach (var result in results)
        {
            if (result.attachedRigidbody && result.attachedRigidbody.TryGetComponent(out BurnableObject burnableObject) && result.attachedRigidbody.TryGetComponent(out EntityHealth entityHealth))
            {
                var distance = (result.transform.position - position).magnitude;
                entityHealth.Health -= burnableObject.BurningDamageMultiplier * ExplosionFalloff.Evaluate(1 - Mathf.Clamp01(distance / ExplosionRadius)) * ExplosionDamage * Time.deltaTime;
            }
        }

        var timer = 0f;
        while (timer < ExplosionDuration)
        {
            foreach (var result in results)
            {
                if (result.attachedRigidbody)
                {
                    var directionMagnitude = result.transform.position - position;
                    var direction = directionMagnitude.normalized;
                    var distance = directionMagnitude.magnitude;

                    result.attachedRigidbody.AddForce(ExplosionForce * ExplosionFalloff.Evaluate(1 - Mathf.Clamp01(distance / ExplosionRadius)) * direction);
                }
            }

            timer += TimeBetweenExplosionTicks;

            yield return new WaitForSeconds(TimeBetweenExplosionTicks);
        }

        Destroy(lifetime);
    }
}
