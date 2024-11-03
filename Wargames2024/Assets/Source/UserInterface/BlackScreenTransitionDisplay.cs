using Cysharp.Threading.Tasks;
using DG.Tweening;
using Exanite.Core.Components;
using Exanite.Core.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Source.UserInterface
{
    public class BlackScreenTransitionDisplay : SingletonBehaviour<BlackScreenTransitionDisplay>
    {
        public Image Image;

        public float SceneLoadDuration = 1f;
        public float DeathDuration = 3f;
        public float TeleportDuration = 1;

        private float opacity = 1;

        private Tweener current;

        protected override void OnEnable()
        {
            base.OnEnable();

            SceneManager.sceneLoaded += OnSceneLoaded;
            opacity = 1;

            Fade(0, SceneLoadDuration).Forget();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            var color = Image.color;
            color.a = opacity;
            Image.color = color;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            opacity = 1;
        }

        public async UniTask Fade(float newOpacity, float duration)
        {
            $"Fading to {newOpacity} in {duration} seconds".Dump();

            current?.Kill();
            current = DOTween.To(() => opacity, value => opacity = value, newOpacity, duration);

            await current.AsyncWaitForCompletion();
        }
    }
}
