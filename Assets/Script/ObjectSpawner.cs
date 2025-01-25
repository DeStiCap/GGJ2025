using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> spawnedObjects = new List<GameObject>();

    public float frequency;
    private bool _spawning = true;
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    
    private IEnumerator SpawnRoutine()
    {
        while (_spawning == true)
        {
            yield return new WaitForSeconds(frequency);

            // Get the camera's world space dimensions
            Camera mainCamera = Camera.main;
            float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;
            float screenHeight = mainCamera.orthographicSize;

            // Randomly pick one of the four edges
            int edge = Random.Range(0, 4); // 0 = Top, 1 = Bottom, 2 = Left, 3 = Right

            Vector3 spawnPosition = Vector3.zero;

            // Get the camera's current position
            Vector3 cameraPosition = mainCamera.transform.position;

            switch (edge)
            {
                case 0: // Top edge
                    spawnPosition = new Vector3(Random.Range(-screenWidth, screenWidth), cameraPosition.y + screenHeight, 0);
                    break;
                case 1: // Bottom edge
                    spawnPosition = new Vector3(Random.Range(-screenWidth, screenWidth), cameraPosition.y - screenHeight, 0);
                    break;
                case 2: // Left edge
                    spawnPosition = new Vector3(cameraPosition.x - screenWidth, Random.Range(-screenHeight, screenHeight), 0);
                    break;
                case 3: // Right edge
                    spawnPosition = new Vector3(cameraPosition.x + screenWidth, Random.Range(-screenHeight, screenHeight), 0);
                    break;
            }

            GameObject obstacle = spawnedObjects[Random.Range(0, spawnedObjects.Count)];

            GameObject newObject = Instantiate(obstacle, spawnPosition, Quaternion.identity);
            newObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(gameObject.transform.position.x - spawnPosition.x,
                gameObject.transform.position.y - spawnPosition.y) * 0.1f;
            newObject.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(0f, 45f);
            Destroy(newObject, 20f);
        }
    }
}
