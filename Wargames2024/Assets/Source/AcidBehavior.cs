using DG.Tweening;
using Exanite.Core.Pooling;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class AcidBehavior : MonoBehaviour
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
    [Space]
    public float MinSpreadRange = 1;
    public float MaxSpreadRange = 2;

    [Space]
    public int SpreadTryCount = 3;
    public float SpreadCheckRadius = 0.5f;

    [Space]
    public float BurnRadius = 0.5f;

    [Space]
    public float MinSpreadTime = 0.5f;
    public float MaxSpreadTime = 2;

    private float spreadTimer;
    private float nextSpreadTime;

    private float timeAlive;

    private float lightIntensity;
    public Vector3 startPosition;
    public float spreadDistance;
    private bool first = true;

    private void Start() {
        UpdateSpreadTime();
        if (first) {
            startPosition = transform.position;
        }
        lightIntensity = Light.intensity;
        Light.intensity = 0;
        DOTween.To(() => Light.intensity, value => Light.intensity = value, lightIntensity, 5);
        AudioEmitter.SetParameter("FireIntensity", 1);
    }

    private void FixedUpdate() {
        // Burn
        using var _ = ListPool<Collider2D>.Acquire(out var results);
        Physics2D.OverlapCircle(transform.position, BurnRadius, default, results);
        foreach (var result in results) {
            if (result.attachedRigidbody && result.attachedRigidbody.TryGetComponent(out MeltableObject meltableObject) && result.attachedRigidbody.TryGetComponent(out EntityHealth entityHealth)) {
                if (result.attachedRigidbody.TryGetComponent(out PlayerCharacter player) && player.statusEffects[1,0] > 0) { 
                    continue;
                }
                entityHealth.Health -= meltableObject.MeltDamageMultiplier * DamagePerSecond * Time.deltaTime;
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
            var spreadPosition = transform.position + (Vector3)(Random.insideUnitCircle.normalized * spreadRange);

            if (Physics2D.OverlapCircle(spreadPosition, SpreadCheckRadius) || (spreadPosition - startPosition).magnitude > spreadDistance)  {
                continue;
            }

            AcidBehavior newAcid = Instantiate(GameContext.Instance.AcidPrefab, spreadPosition, Quaternion.identity);
            newAcid.first = false;

            break;
        }
    }
    private void UpdateSpreadTime() {
        nextSpreadTime = Random.Range(MinSpreadTime, MaxSpreadTime);
    }
}
