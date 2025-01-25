using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    public Vector3 minValue;
    public Vector3 maxValue;
    
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