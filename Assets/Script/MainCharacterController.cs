using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MainCharacterController : MonoBehaviour
{
    
    public float spriteScale = 5;
    public float moveSpeed = 5;
    public float randomSpeedRange = 0.2f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 100f;
    
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
        UpdateSpriteDirection();
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
            // Shoot from character, direction is determined by character and mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 characterToMouseDirection = mousePosition - rigidbody.position;

            GameObject bullet = GameObject.Instantiate(bulletPrefab, rigidbody.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = characterToMouseDirection.normalized * bulletSpeed;
            Destroy(bullet, 5f);
        }
    }

    private void UpdateSpriteDirection()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 characterToMouseDirection = mousePosition - rigidbody.position;

        if (characterToMouseDirection.x < 0)
        {
            transform.localScale = new Vector3(-spriteScale, spriteScale, 1);
        }
        else
        {
            transform.localScale = new Vector3(spriteScale, spriteScale, 1);
        }
    }
}
