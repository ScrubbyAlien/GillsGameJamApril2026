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
    private Collider2D col;
    [SerializeField]
    private Behaviour platformBehaviour;

    private Collider2D playerFeet;

    private void Start()
    {
        playerFeet = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        switch (platformBehaviour)
        {
            case Behaviour.AlwaysOn: return;
            case Behaviour.PassThroughBottom:
                Debug.DrawLine(col.bounds.max, playerFeet.bounds.min, Color.red, Time.fixedDeltaTime);
                if (col.bounds.max.y > playerFeet.bounds.min.y)
                {
                    col.isTrigger = true;
                }
                else col.isTrigger = false;
                break;
        }
    }
}