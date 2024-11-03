using System.Numerics;
using DG.Tweening;
using Exanite.Core.Pooling;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class FireBehaviour : MonoBehaviour
{
    // Fire can spread, ignite, and burn
    // Spread: Creates a new copy of the fire in a random location around the fire
    // Ignite: Sets things the fire touches on fire, if it is not already on fire
    // Burn: Damages things that are nearby, if they are burnable

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
    public float SpreadCheckRadius = 0.5f;

    [Space]
    public float BurnRadius = 0.5f;

    [Space]
    public float MinSpreadTime = 1;
    public float MaxSpreadTime = 5;

    private float spreadTimer;
    private float nextSpreadTime;

    private float timeAlive;

    private float lightIntensity;

    private void Start() {
        UpdateSpreadTime();

        lightIntensity = Light.intensity;
        Light.intensity = 0;
        DOTween.To(() => Light.intensity, value => Light.intensity = value, lightIntensity, 5);
        AudioEmitter.SetParameter("FireIntensity", 1);

        CheckForResistantObjects();
    }

    private void FixedUpdate() {
        // Burn
        using var _ = ListPool<Collider2D>.Acquire(out var results);
        Physics2D.OverlapCircle(transform.position, BurnRadius, default, results);
        foreach (var result in results) {
            if (result.attachedRigidbody && result.attachedRigidbody.TryGetComponent(out BurnableObject burnableObject) && result.attachedRigidbody.TryGetComponent(out EntityHealth entityHealth)) {
                entityHealth.Health -= burnableObject.BurningDamageMultiplier * DamagePerSecond * Time.deltaTime;
            }
        }
    }

    private void Update() {
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

    private void TrySpread() {
        for (var i = 0; i < SpreadTryCount; i++) {
            var spreadRange = Random.Range(MinSpreadRange, MaxSpreadRange);
            var spreadPosition = transform.position + (UnityEngine.Vector3)(Random.insideUnitCircle.normalized * spreadRange);

            if (Physics2D.OverlapCircle(spreadPosition, SpreadCheckRadius))  {
                continue;
            }

            Instantiate(GameContext.Instance.FirePrefab, spreadPosition, UnityEngine.Quaternion.identity);

            break;
        }

        using var _ = ListPool<Collider2D>.Acquire(out var results);
        Physics2D.OverlapCircle(transform.position, BurnRadius * 2, default, results);
        foreach (var result in results) {
            if (result.attachedRigidbody && result.attachedRigidbody.TryGetComponent(out BurnableObject burnableObject) && result.attachedRigidbody.TryGetComponent(out EntityHealth entityHealth)  && !result.attachedRigidbody.TryGetComponent(out PlayerCharacter player)) {
                SpreadAt(result.attachedRigidbody.transform.position);
            }
        }
    }

    private void SpreadAt(UnityEngine.Vector3 position) {
        var spreadRange = Random.Range(0, 2);
        var spreadPosition = position + (UnityEngine.Vector3)(Random.insideUnitCircle.normalized * spreadRange);
        Instantiate(GameContext.Instance.FirePrefab, spreadPosition, UnityEngine.Quaternion.identity);
    }
    private void UpdateSpreadTime() {
        nextSpreadTime = Random.Range(MinSpreadTime, MaxSpreadTime);
    }

    private void CheckForResistantObjects() {
        using var _ = ListPool<Collider2D>.Acquire(out var results);
        Physics2D.OverlapCircle(transform.position, BurnRadius, default, results);
        foreach (var result in results) {
            if (result.attachedRigidbody && result.attachedRigidbody.TryGetComponent(out FireResist resistantObject)) {
                if (resistantObject.FireResistRadius >= resistantObject.attachedRigidbody.transform.position - transform.position) {
                    Object.Destroy(this.gameObject);
                }
            }
        }
    }
}
