using Cysharp.Threading.Tasks;
using Source.UserInterface;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Dependencies")]
    public InputActionReference MovementInput;
    public Rigidbody2D Rigidbody;
    public EntityHealth Health;
    public GameObject Sprite;

    public Sprite[] Animation;
    public SpriteRenderer SpriteRenderer;
    public float MovementSpeed = 10;
    public float MovementSmoothTime = 0.3f;

    public float rotationSpeed = 180f;
    public float animationSpeed = 5;
    public float[,] statusEffects = {{0,0},{0,0}};

    private Vector3 referenceVelocity;

    private GameContext gameContext;
    private float frame = 0;

    private static bool IsTransitioning = false;

    private void OnEnable() {
        gameContext = GameContext.Instance;
        gameContext.Player = this;
        IsTransitioning = false;
    }

    private void OnDisable() {
        if (gameContext.Player == this)  {
            gameContext.Player = null;
        }
    }

    private void Update() {
        var targetVelocity = MovementInput.action.ReadValue<Vector2>();
        targetVelocity *= MovementSpeed;
        Rigidbody.velocity = Vector3.SmoothDamp(Rigidbody.velocity, targetVelocity, ref referenceVelocity, MovementSmoothTime);

        if (targetVelocity != Vector2.zero) {
            var angleDegrees = Mathf.Atan2(targetVelocity.y, targetVelocity.x) * Mathf.Rad2Deg;
            var targetRotation = Quaternion.Euler(0, 0, angleDegrees);

            Sprite.transform.rotation = Quaternion.Slerp(targetRotation, Sprite.transform.rotation, Mathf.Pow(0.5f, rotationSpeed * Time.deltaTime));
            frame += Time.deltaTime * animationSpeed;
            if (frame >= 3) {
                frame = 0.5f;
            } else if (frame <= 0.5) {
                frame = 0.5f;
            }
        } else {
            frame = 0;
        }
        SpriteRenderer.sprite = Animation[(int) frame];
        for (int i = 0; i < 2; i++) {
            if (statusEffects[i,0] <= 0) {
                statusEffects[i,0] = 0;
                if (i == 0) {
                    UiContext.Instance.regenIcon.enabled = false;
                } else if (i == 1) {
                    UiContext.Instance.resistIcon.enabled = false;
                }
            } else if (statusEffects[i,0] > 0) {
                statusEffects[i,0] -= Time.deltaTime;
                if (i == 0) {
                    UiContext.Instance.regenIcon.enabled = true;
                    Health.Health += statusEffects[i,1] * Time.deltaTime;
                } else if (i == 1) {
                    UiContext.Instance.resistIcon.enabled = true;
                }
            }
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            if (!IsTransitioning)
            {
                GameContext.Instance.IsLevelTimerPaused = true;
                BlackScreenTransitionDisplay.Instance.Fade(1, BlackScreenTransitionDisplay.Instance.DeathDuration).ContinueWith(() =>
                {
                    SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
                    IsTransitioning = false;
                }).Forget();
            }

            IsTransitioning = true;
        }
        else
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            if (!IsTransitioning)
            {
                GameContext.Instance.IsLevelTimerPaused = true;
                BlackScreenTransitionDisplay.Instance.Fade(1, BlackScreenTransitionDisplay.Instance.DeathDuration).ContinueWith(() =>
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                    IsTransitioning = false;
                }).Forget();
            }

            IsTransitioning = true;
        }
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }

        GameContext.Instance.IsLevelTimerPaused = true;
        BlackScreenTransitionDisplay.Instance.Fade(1, BlackScreenTransitionDisplay.Instance.DeathDuration).ContinueWith(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }).Forget();
    }
}
