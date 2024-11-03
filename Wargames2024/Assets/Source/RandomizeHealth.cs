using UnityEngine;
using Random = UnityEngine.Random;

namespace Source
{
    public class RandomizeHealth : MonoBehaviour
    {
        public EntityHealth Health;

        public float MinHealth;
        public float MaxHealth;

        private void Start()
        {
            Health.MaxHealth = Random.Range(MinHealth, MaxHealth);
            Health.Health = Health.MaxHealth;
        }
    }
}
