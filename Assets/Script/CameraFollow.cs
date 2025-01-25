using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    public Vector3 minValue = new Vector3(-51.4f, -32.6f, 0f);
    public Vector3 maxValue = new Vector3(55.1f, 24.4f, 0f);
    
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(playerPos.x, minValue.x, maxValue.x),
            Mathf.Clamp(playerPos.y, minValue.y, maxValue.y),
            Mathf.Clamp(playerPos.z, minValue.z, maxValue.z));
        gameObject.transform.position = new Vector3 (boundPosition.x + offset.x, boundPosition.y + offset.y, offset.z); // Camera follows the player with specified offset position
    }
}