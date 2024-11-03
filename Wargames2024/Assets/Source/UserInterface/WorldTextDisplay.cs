using TMPro;
using UnityEngine;

namespace Source.UserInterface
{
    public class WorldTextDisplay : MonoBehaviour
    {
        public TMP_Text Text;
        public AnimationCurve OpacityCurve = new();

        public float DisplayRange = 2;
        public float FadeRange = 5;

        private void Update()
        {
            var distance = float.PositiveInfinity;

            var player = GameContext.Instance.Player;
            if (player != null)
            {
                distance = (player.transform.position - transform.position).magnitude;
            }

            var color = Text.color;
            color.a = OpacityCurve.Evaluate(1 - Mathf.Clamp01((distance - DisplayRange) / (FadeRange - DisplayRange)));
            Text.color = color;
        }
    }
}
