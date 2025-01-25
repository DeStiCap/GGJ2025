using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    
    void Update()
    {
        Vector3 playerPos = player.transform.position;;
        gameObject.transform.position = new Vector3 (playerPos.x + offset.x, playerPos.y + offset.y, offset.z); // Camera follows the player with specified offset position
    }
}