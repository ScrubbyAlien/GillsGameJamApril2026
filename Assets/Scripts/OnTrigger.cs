using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    public UnityEvent<Collider2D> TriggerEnter;
    public UnityEvent<Collider2D> TriggerExit;
    public UnityEvent<Collider2D> TriggerStay;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEnter?.Invoke(other);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        TriggerExit?.Invoke(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        TriggerStay?.Invoke(other);
    }
}