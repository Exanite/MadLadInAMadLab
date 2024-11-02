using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
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

    public float rotationSpeed = 5f;
    private Vector2 _smoothedMovementInput;

    private Vector3 referenceVelocity;

    private GameContext gameContext;
    private Vector2 _movementInput;
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
        if (targetVelocity != Vector2.zero) {
            Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0);

            Quaternion targetRotation = Quaternion.LookRotation(inputVector, Vector3.back); 
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            sprite.transform.rotation = rotation;
            print(rotation);
        }
    }
}
