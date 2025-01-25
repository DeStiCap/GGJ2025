using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainCharacterController : MonoBehaviour
{

    public float spriteScale = 5;
    public float moveSpeed = 5;
    public float randomSpeedRange = 0.2f;
    public float invulnerableTime = 5;
    public float maxHealth = 100;
    public GameObject healthBar;
    public GameObject cooldownBar;

    [System.Serializable]
    public class WeaponGunProperty
    {
        public bool isActive = true;
        public GameObject bulletPrefab;
        public float bulletSpeed = 100f;
        public float cooldownTime = 0.5f;

        public float lastFireTime;
        public bool isOnCooldown = false;
    }

    [System.Serializable]
    public class WeaponSpreadProperty
    {
        public bool isActive = false;
        public GameObject bulletPrefab;
        public int spreadBulletNumber = 18;
        public float bulletSpeed = 70f;
        public float cooldownTime = 1f;

        public float lastFireTime;
        public bool isOnCooldown = false;
    }
    
    [System.Serializable]
    public class WeaponLandmineProperty
    {
        public bool isActive = false;
        public GameObject bulletPrefab;
        public float cooldownTime = 5f;

        public float lastFireTime;
        public bool isOnCooldown = false;
    }

    public WeaponGunProperty weaponGun;
    public WeaponSpreadProperty weaponSpread;
    [FormerlySerializedAs("WeaponLandmine")] public WeaponLandmineProperty weaponLandmine;

    private Rigidbody2D rigidbody;
    private float health;
    private float lastHitTime;
    private bool isInvulnerable;
    private Slider healthBarSlider;
    private Slider cooldownBarSlider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        healthBarSlider = healthBar.GetComponent<Slider>();
        cooldownBarSlider = cooldownBar.GetComponent<Slider>();

        health = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isInvulnerable)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                isInvulnerable = true;
                StartCoroutine(Flash());
                lastHitTime = Time.time;
                health -= 20;
            }
            else if (other.gameObject.CompareTag("EnemyBullet"))
            {
                isInvulnerable = true;
                StartCoroutine(Flash());
                lastHitTime = Time.time;
                Destroy(other.gameObject);
                health -= 20;
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
        
        UpdateUIElement();
        MovementControl();
        WeaponControl();
        UpdateSprite();
    }

    private void UpdateUIElement()
    {
        healthBarSlider.value = health * (healthBarSlider.maxValue / maxHealth);
        
        cooldownBar.SetActive(false);
        
        // if (weaponSpread.isOnCooldown)
        // {
        //     cooldownBar.SetActive(true);
        //     cooldownBarSlider.value = (weaponSpread.lastFireTime + weaponSpread.cooldownTime - Time.time) / 
        //                               weaponSpread.cooldownTime;
        // }
        // else
        // {
        //     cooldownBar.SetActive(false);
        // }

        // if (weaponLandmine.isOnCooldown)
        // {
        //     cooldownBar.SetActive(true);
        //     cooldownBarSlider.value = (weaponLandmine.lastFireTime + weaponLandmine.cooldownTime - Time.time) /
        //                               weaponLandmine.cooldownTime;
        // }
        // else
        // {
        //     cooldownBar.SetActive(false);
        // }
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
        UpdateWeaponCooldownState();
        
        // Fire Projectile Bullet
        if (weaponGun.isActive && !weaponGun.isOnCooldown && Input.GetMouseButtonDown(0))
        {
            // Shoot from character, direction is determined by character and mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 characterToMouseDirection = mousePosition - rigidbody.position;

            GameObject bullet = GameObject.Instantiate(weaponGun.bulletPrefab, rigidbody.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = characterToMouseDirection.normalized * weaponGun.bulletSpeed;
            Destroy(bullet, 5f);
            weaponGun.lastFireTime = Time.time;
            weaponGun.isOnCooldown = true;
        }

        // Fire Spread Bullets
        if (weaponSpread.isActive && !weaponSpread.isOnCooldown && Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < 360; i += weaponSpread.spreadBulletNumber) {
                double angle1 = Math.Cos(DegreeToRadian(i));
                double angle2 = Math.Sin(DegreeToRadian(i));
                Vector2 projposition = new Vector2((float) angle1, (float) angle2);
                
                GameObject bullet = GameObject.Instantiate(weaponSpread.bulletPrefab,rigidbody.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().linearVelocity = projposition.normalized * weaponSpread.bulletSpeed;
                Destroy(bullet, 0.2f);
                weaponSpread.lastFireTime = Time.time;
                weaponSpread.isOnCooldown = true;
            }
        }
        
        // Landmine
        if (weaponLandmine.isActive && !weaponLandmine.isOnCooldown && Input.GetMouseButtonDown(2))
        {
            GameObject landmine = GameObject.Instantiate(weaponGun.bulletPrefab, rigidbody.position, Quaternion.identity);
            Destroy(landmine, 5f);
            weaponLandmine.lastFireTime = Time.time;
            weaponLandmine.isOnCooldown = true;
        }
    }

    private void UpdateWeaponCooldownState()
    {
        if (Time.time - weaponGun.lastFireTime > weaponGun.cooldownTime)
        {
            weaponGun.isOnCooldown = false;
        }

        if (Time.time - weaponSpread.lastFireTime > weaponSpread.cooldownTime)
        {
            weaponSpread.isOnCooldown = false;
        }

        if (Time.time - weaponLandmine.lastFireTime > weaponLandmine.cooldownTime)
        {
            weaponLandmine.isOnCooldown = false;
        }
    }
    
    private double DegreeToRadian(double a)
    {
        return Math.PI * (a/180.0);
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
