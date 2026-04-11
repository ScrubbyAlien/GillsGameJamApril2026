using UnityEngine;

public class Tomato : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private ParticleSystem splatEffect;
    [SerializeField]
    private Sensor groundSensor;

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float aggressiveWalkSpeed;
    private float direction;

    private Transform player;

    public enum Hostility
    {
        Hostile,
        Neutral
    }

    public enum Behaviour
    {
        Idle,
        Patrolling,
        Aggressive
    }

    private static Hostility hostility;
    [SerializeField]
    private Behaviour mode;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        hostility = Hostility.Neutral;
        direction = Mathf.Sign(walkSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hostility == Hostility.Hostile)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                player.GetComponent<PlayerController>().TakeDamage(1f);
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
        {
            Instantiate(splatEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            hostility = Hostility.Hostile;
        }
    }

    private void FixedUpdate()
    {
        UpdateDirection();
        float velocity = direction * WalkSpeed();
        if (velocity < 0f) Flip(true);
        else if (velocity > 0f) Flip(false);
        body.linearVelocityX = velocity;
        // animator.SetFloat("speed", Mathf.Abs(velocity));
    }

    private void UpdateDirection()
    {
        switch (mode)
        {
            case Behaviour.Idle:
                direction = 0;
                break;
            case Behaviour.Patrolling:
                if (!groundSensor.sensing) direction = -direction;
                break;
            case Behaviour.Aggressive:
                if (hostility == Hostility.Neutral) goto case Behaviour.Patrolling;
                if (!player) goto case Behaviour.Idle;
                direction = Mathf.Sign(player.position.x - transform.position.x);
                break;
        }
    }

    private float WalkSpeed()
    {
        if (hostility == Hostility.Hostile && mode == Behaviour.Aggressive) return aggressiveWalkSpeed;
        else return walkSpeed;
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
}