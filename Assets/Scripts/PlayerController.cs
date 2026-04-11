using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event Action<float, float> UpdateHitPoints;
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

    [Header("Hit points")]
    [SerializeField]
    private float maxHitPoints;
    private float currentHitPoints;
    [SerializeField]
    private float invinciblityTime;
    private float invincibleUntil;

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

    private void Start()
    {
        currentHitPoints = maxHitPoints;
        UpdateHitPoints?.Invoke(currentHitPoints, maxHitPoints);
    }

    private void FixedUpdate()
    {
        velocity = Mathf.Lerp(velocity, walkVelocity * direction, dampening);
        if (velocity < 0f) Flip(true);
        else if (velocity > 0f) Flip(false);
        body.linearVelocityX = velocity;
        animator.SetFloat("speed", Mathf.Abs(velocity));

        if (body.linearVelocityY < 0) body.gravityScale = jumpFallGravity;
        else body.gravityScale = jumpRiseGravity;
    }

    private void Flip(bool left)
    {
        Vector3 scale = transform.localScale;
        float absXScale = Mathf.Abs(scale.x);
        Vector3 flippedScale;
        if (left) flippedScale = new Vector3(-absXScale, scale.y, scale.z);
        else flippedScale = new Vector3(absXScale, scale.y, scale.z);
        transform.localScale = flippedScale;
    }

    public void TakeDamage(float damage)
    {
        if (Time.time <= invincibleUntil) return;
        currentHitPoints -= damage;
        if (currentHitPoints <= 0f)
        {
            Destroy(gameObject);
        }
        invincibleUntil = Time.time + invinciblityTime;
    }
}