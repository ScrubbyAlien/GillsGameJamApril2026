using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollower : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float dampening;
    private Transform playerFeet;
    private float zPos;

    [SerializeField]
    private float screenHeight;
    private float halfHeight => screenHeight / 2;
    private int screenIndex;
    [SerializeField]
    private float minIndex;

    private void Start()
    {
        playerFeet = GameObject.FindWithTag("Player").GetComponent<PlayerController>().feet;
        zPos = transform.position.z;
    }

    public void LateUpdate()
    {
        float levelHeight = playerFeet.position.y + halfHeight;
        screenIndex = Mathf.FloorToInt(levelHeight / screenHeight);
        Vector3 targetPosition = new Vector3(0, screenIndex * screenHeight, zPos);
        transform.position = Vector3.Lerp(transform.position, targetPosition, dampening * Time.deltaTime * 100);
    }
}