using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainCharacterController : MonoBehaviour
{
    
    public float spriteScale = 5;
    public float moveSpeed = 5;
    public float randomSpeedRange = 0.2f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 100f;
    public float invulnerableTime = 5;
    
    private Rigidbody2D rigidbody;
    private float lastHitTime;
    private bool isInvulnerable;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isInvulnerable)
        {
            isInvulnerable = true;
            StartCoroutine(Flash());
            if (other.gameObject.CompareTag("Enemy"))
            {
                lastHitTime = Time.time;
                Debug.Log("Player Take Damage");
            }
            else if (other.gameObject.CompareTag("EnemyBullet"))
            {
                lastHitTime = Time.time;
                Destroy(other.gameObject);
                Debug.Log("Player Take Damage");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastHitTime > invulnerableTime)
        {
            isInvulnerable = false;
        }
        
        MovementControl();
        WeaponControl();
        UpdateSprite();
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

    private void UpdateSprite()
    {
        // Update facing direction
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
    
    private IEnumerator Flash() {
        SpriteRenderer playerSprite =  gameObject.GetComponent<SpriteRenderer>();

        for (int i = 0; i < invulnerableTime * 10; i++)
        {
            playerSprite.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.05f);
            playerSprite.color = new Color(1, 1, 1, 1);;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
