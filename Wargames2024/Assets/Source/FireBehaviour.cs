using System.Linq;
using DG.Tweening;
using Exanite.Core.Pooling;
using Exanite.Core.Utilities;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FireBehaviour : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    public Collider2D Collider;
    public Light2D Light;
    public StudioEventEmitter AudioEmitter;

    public float DamagePerSecond = 10;
    public float Duration = 5;

    [Space]
    public float MinSpreadRange = 2;
    public float MaxSpreadRange = 4;

    [Space]
    public int SpreadTryCount = 3;
    [FormerlySerializedAs("SpreadCheckRadius")]
    public float SpreadDenialRadius = 0.5f;

    [Space]
    public float DamageRadius = 0.5f;
    public float DamageTickTime = 0.25f;

    [Space]
    public float MinSpreadTime = 1;
    public float MaxSpreadTime = 5;

    private float spreadTimer;
    private float nextSpreadTime;

    private float timeAlive;

    private float lightIntensity;

    private float damageTimer = 0;

    private void Start() {
        UpdateSpreadTime();

        lightIntensity = Light.intensity;
        Light.intensity = 0;
        DOTween.To(() => Light.intensity, value => Light.intensity = value, lightIntensity, 5);
        AudioEmitter.SetParameter("FireIntensity", 1);

        CheckForResistantObjects();
    }

    private void Update() {
        // Damage
        damageTimer += Time.deltaTime;
        if (damageTimer > DamageTickTime)
        {
            damageTimer -= DamageTickTime;
            using var _ = ListPool<Collider2D>.Acquire(out var results);
            Physics2D.OverlapCircle(transform.position, DamageRadius, default, results);
            foreach (var result in results) {
                if (result.attachedRigidbody && result.attachedRigidbody.TryGetComponent(out BurnableObject damageableObject) && result.attachedRigidbody.TryGetComponent(out EntityHealth entityHealth)) {
                    if (result.attachedRigidbody.TryGetComponent(out PlayerCharacter player) && player.statusEffects[1,0] > 0) {
                        continue;
                    }
                    entityHealth.Health -= damageableObject.BurningDamageMultiplier * DamagePerSecond * DamageTickTime;
                }
            }
        }

        // Spread
        spreadTimer += Time.deltaTime;
        if (spreadTimer > nextSpreadTime) {
            TrySpread();
            UpdateSpreadTime();

            spreadTimer = 0;
        }

        // Lifetime
        timeAlive += Time.deltaTime;
        if (timeAlive > Duration) {
            Stop();
        }
    }

    public void Stop() {
        if (enabled) {
            DOTween.To(() => Light.intensity, value => Light.intensity = value, 0, 8);
            Collider.enabled = false;
            enabled = false;
            ParticleSystem.Stop();
            AudioEmitter.SetParameter("FireIntensity", 0);
            Destroy(gameObject, 10);
        }
    }

    private void TrySpread()
    {
        for (var i = 0; i < SpreadTryCount; i++) {
            var spreadRange = Random.Range(MinSpreadRange, MaxSpreadRange);
            var spreadPosition = transform.position + (Vector3)(Random.insideUnitCircle.normalized * spreadRange);

            TrySpreadAt(spreadPosition);
        }

        using var _ = ListPool<Collider2D>.Acquire(out var results2);
        Physics2D.OverlapCircle(transform.position, DamageRadius, default, results2);
        foreach (var result in results2) {
            if (result.attachedRigidbody && result.attachedRigidbody.TryGetComponent(out BurnableObject _) && !result.attachedRigidbody.TryGetComponent(out PlayerCharacter _)) {
                var spreadRange = Random.Range(0, 2);
                var spreadPosition = result.attachedRigidbody.transform.position + (Vector3)(Random.insideUnitCircle.normalized * spreadRange);

                TrySpreadAt(spreadPosition.Dump());
            }
        }
    }

    private void TrySpreadAt(Vector3 position)
    {
        using var _ = ListPool<RaycastHit2D>.Acquire(out var hits);
        Physics2D.Linecast(transform.position, position, new ContactFilter2D()
        {
            useTriggers = false,
        }, hits);

        foreach (var hit in hits)
        {
            if (!hit.collider.TryGetComponent(out BurnableObject _) || !(hit.collider.attachedRigidbody && hit.collider.attachedRigidbody.TryGetComponent(out BurnableObject _)))
            {
                return;
            }
        }

        using var __ = ListPool<Collider2D>.Acquire(out var colliders);
        Physics2D.OverlapCircle(position, SpreadDenialRadius, new ContactFilter2D()
        {
            useTriggers = true,
        }, colliders);

        var isSpreadBlocked = colliders.Any(static (c) =>
        {
            if (c.TryGetComponent(out FireBehaviour _))
            {
                return true;
            }

            if (c.TryGetComponent(out BurnableObject _) || (c.attachedRigidbody && c.attachedRigidbody.TryGetComponent(out BurnableObject _)))
            {
                return false;
            }

            if (c.isTrigger)
            {
                return false;
            }

            return true;
        });

        if (isSpreadBlocked)
        {
            return;
        }

        Instantiate(GameContext.Instance.FirePrefab, position, Quaternion.identity);
    }

    private void UpdateSpreadTime() {
        nextSpreadTime = Random.Range(MinSpreadTime, MaxSpreadTime);
    }

    private void CheckForResistantObjects() {
        using var _ = ListPool<Collider2D>.Acquire(out var results);
        Physics2D.OverlapCircle(transform.position, DamageRadius, default, results);
        foreach (var result in results) {
            if (result.attachedRigidbody && result.attachedRigidbody.TryGetComponent(out FireResist resistantObject)) {
                if (resistantObject.FireResistRadius >= (result.attachedRigidbody.transform.position - transform.position).magnitude) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
