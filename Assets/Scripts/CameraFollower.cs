using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollower : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float dampening;
    private Transform player;
    private float zPos;

    [SerializeField]
    private float minY, maxY;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        zPos = transform.position.z;
    }

    public void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(0, Mathf.Clamp(player.position.y, minY, maxY), zPos);
        transform.position = Vector3.Lerp(transform.position, targetPosition, dampening * Time.deltaTime * 100);
    }
}