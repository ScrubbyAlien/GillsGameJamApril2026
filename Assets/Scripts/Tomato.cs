using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Tomato : MonoBehaviour, IKillable
{
    [SerializeField]
    private WorldState worldState;

    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private ParticleSystem splatEffect;
    [SerializeField]
    private Sensor groundSensor;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform spriteTransform;

    [SerializeField]
    private float walkSpeed;
    private float direction;
    private float rotation;

    [SerializeField]
    public bool aggressive = true;
    [SerializeField]
    private UnityEvent OnTurnAggressive;
    [SerializeField]
    private UnityEvent OnDeath;

    private Transform player;

    public enum Hostility
    {
        Hostile,
        Neutral
    }

    private Hostility hostility;
    private void TurnAggressive()
    {
        StartCoroutine(TurnWhenPlayerNear());
    }

    private IEnumerator TurnWhenPlayerNear()
    {
        yield return new WaitUntil(() => Vector3.Distance(transform.position, player.position) < 12);
        hostility = Hostility.Hostile;
        worldState.OnTomatoKilled -= TurnAggressive;
        animator.SetTrigger("attack");
        OnTurnAggressive?.Invoke();
    }

    public void StartRolling()
    {
        direction = Mathf.Sign(walkSpeed);
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        hostility = Hostility.Neutral;
        direction = Mathf.Sign(walkSpeed);
        worldState.RegisterTomato(this);
        if (aggressive) worldState.OnTomatoKilled += TurnAggressive;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hostility == Hostility.Hostile)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                player.GetComponent<PlayerController>().TakeDamage(1f, transform.position);
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        UpdateDirection();
        float velocity = direction * walkSpeed;
        rotation = velocity / body.GetComponent<CircleCollider2D>().radius;
        if (velocity < 0f) Flip(true);
        else if (velocity > 0f) Flip(false);
        body.linearVelocityX = velocity;
    }

    private void Update()
    {
        Vector3 ea = spriteTransform.eulerAngles;
        float rotZ = ea.z - rotation * Mathf.Rad2Deg * Time.deltaTime * 2;
        spriteTransform.eulerAngles = new Vector3(ea.x, ea.y, rotZ);
    }

    private void UpdateDirection()
    {
        switch (hostility)
        {
            case Hostility.Neutral:
                direction = 0;
                break;
            case Hostility.Hostile:
                if (!groundSensor.sensing) direction = -direction;
                break;
        }
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

    public void Die()
    {
        worldState.KillTomato(this);
        worldState.OnTomatoKilled -= TurnAggressive;
        OnDeath?.Invoke();
        Instantiate(splatEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}