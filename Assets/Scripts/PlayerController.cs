using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event Action Interact;

    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    [SerializeField]
    private float walkVelocity;
    [SerializeField, Range(0f, 1f)]
    private float dampening;

    [Header("Jumping")]
    // https://www.youtube.com/watch?v=IOe1aGY6hXA&list=PLGKTwK4yOQ_dpXp3FCPumQAwf-6fjERKe&index=1
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float timeToPeak;
    [SerializeField]
    private float timeToLand;
    [SerializeField]
    private Sensor floorSensor;
    public Transform feet => floorSensor.transform;
    [SerializeField]
    private float jumpCancelledBreakFactor;

    private float jumpVelocity => 2 * jumpHeight / timeToPeak;
    private float jumpRiseGravity => -jumpVelocity / timeToPeak;
    private float jumpFallGravity => -jumpVelocity * timeToPeak / (timeToLand * timeToLand);

    private float direction;
    private float velocity;

    [SerializeField]
    private bool hasSword;

    [HideInInspector]
    public bool locked;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (locked)
        {
            direction = 0;
            return;
        }
        direction = Vector2.Dot(context.ReadValue<Vector2>(), Vector2.right);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (locked) return;
        if (context.performed)
        {
            if (!floorSensor.sensing) return;
            body.linearVelocityY = jumpVelocity;
        }
        if (context.canceled)
        {
            if (body.linearVelocityY is > 0) body.linearVelocityY *= jumpCancelledBreakFactor;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started) Interact?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.started || locked || !hasSword) return;
        animator.SetTrigger("attack");
    }

    private void FixedUpdate()
    {
        velocity = Mathf.Lerp(velocity, walkVelocity * direction, dampening);
        if (velocity < 0f) spriteRenderer.flipX = true;
        else if (velocity > 0f) spriteRenderer.flipX = false;
        body.linearVelocityX = velocity;
        animator.SetFloat("speed", Mathf.Abs(velocity));

        if (body.linearVelocityY < 0) body.gravityScale = jumpFallGravity;
        else body.gravityScale = jumpRiseGravity;
    }
}