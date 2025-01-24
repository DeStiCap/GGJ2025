using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MainCharacterController : MonoBehaviour
{
    
    public float moveSpeed = 5;
    public float randomSpeedRange = 0.2f;
    public GameObject bulletPrefab;
    
    private Rigidbody2D rigidbody;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementControl();
        WeaponControl();
    }

    private void MovementControl()
    {
        float randomModifier = Random.Range(1 - randomSpeedRange, 1 + randomSpeedRange);
        float actualMoveSpeed = moveSpeed * randomModifier * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.linearVelocityY += actualMoveSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.linearVelocityX -= actualMoveSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            rigidbody.linearVelocityY -= actualMoveSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.linearVelocityX += actualMoveSpeed;
        }
    }

    private void WeaponControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("FIRE");
        }
    }
}
