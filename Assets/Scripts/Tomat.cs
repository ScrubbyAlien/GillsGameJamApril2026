using UnityEngine;

public class Tomat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
        {
            Destroy(gameObject);
        }
    }
}