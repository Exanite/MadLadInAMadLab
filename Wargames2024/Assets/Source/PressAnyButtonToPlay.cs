using Cysharp.Threading.Tasks;
using Source.UserInterface;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyButtonToPlay : MonoBehaviour
{
    private bool isTransitioning;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer && !Application.isEditor)
        {
            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown && !isTransitioning)
        {
            isTransitioning = true;
            BlackScreenTransitionDisplay.Instance.Fade(1, BlackScreenTransitionDisplay.Instance.DeathDuration)
                .ContinueWith(() =>
                {
                    SceneManager.LoadScene("Level1", LoadSceneMode.Single);
                })
                .Forget();
        }
    }
}
