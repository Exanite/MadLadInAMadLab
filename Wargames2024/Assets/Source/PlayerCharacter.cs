using System.Diagnostics.CodeAnalysis;
using Exanite.Core.Utilities;
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

        Rigidbody.velocity.Dump();

        var angleDegrees = Mathf.Atan2(Rigidbody.velocity.y, Rigidbody.velocity.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(0, 0, angleDegrees);

        sprite.transform.rotation = targetRotation;
    }
}
