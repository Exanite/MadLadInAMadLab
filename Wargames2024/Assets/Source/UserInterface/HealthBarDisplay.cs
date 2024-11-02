using UnityEngine;
using UnityEngine.UI;

namespace Source.UserInterface
{
    public class HealthBarDisplay : MonoBehaviour
    {
        public Image Fill;

        private void Update()
        {
            var player = GameContext.Instance.Player;
            if (!player)
            {
                Fill.fillAmount = 0;

                return;
            }

            Fill.fillAmount = player.Health.Health / player.Health.MaxHealth;
        }
    }
}
