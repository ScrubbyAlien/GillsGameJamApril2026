using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour
{
    private enum Behaviour
    {
        AlwaysOn,
        PassThroughBottom
    }

    [SerializeField]
    private Collider2D collider;
    [SerializeField]
    private Behaviour platformBehaviour;

    private Transform playerFeet;

    private void Start()
    {
        playerFeet = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        switch (platformBehaviour)
        {
            case Behaviour.AlwaysOn: return;
            case Behaviour.PassThroughBottom:
                if (collider.bounds.max.y > playerFeet.position.y)
                {
                    collider.enabled = false;
                }
                else collider.enabled = true;
                break;
        }
    }
}