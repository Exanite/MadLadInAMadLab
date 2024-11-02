using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Dependencies")]
    public InputActionReference MovementInput;
    public Rigidbody2D Rigidbody;
    public EntityHealth Health;
    public GameObject sprite;
    public float MovementSpeed = 10;
    public float MovementSmoothTime = 0.3f;

    public float rotationSpeed = 180f;

    private Vector3 referenceVelocity;

    private GameContext gameContext;

    private void OnEnable()
    {
        gameContext = GameContext.Instance;
        gameContext.Player = this;
    }

    private void OnDisable()
    {
        if (gameContext.Player == this)
        {
            gameContext.Player = null;
        }
    }

    private void Update()
    {
        var targetVelocity = MovementInput.action.ReadValue<Vector2>();
        targetVelocity *= MovementSpeed;
        Rigidbody.velocity = Vector3.SmoothDamp(Rigidbody.velocity, targetVelocity, ref referenceVelocity, MovementSmoothTime);

        if (targetVelocity != Vector2.zero)
        {
            var angleDegrees = Mathf.Atan2(targetVelocity.y, targetVelocity.x) * Mathf.Rad2Deg;
            var targetRotation = Quaternion.Euler(0, 0, angleDegrees);

            sprite.transform.rotation = Quaternion.Slerp(targetRotation, sprite.transform.rotation, Mathf.Pow(0.5f, rotationSpeed * Time.deltaTime));
        }
    }
}
