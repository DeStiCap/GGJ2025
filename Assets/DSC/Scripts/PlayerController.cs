using System;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ2025
{
    public class PlayerController : MonoBehaviour
    {
        #region Data

        [System.Serializable]
        public class WeaponGunProperty
        {
            public bool isActive = true;
            public GameObject bulletPrefab;
            public float bulletSpeed = 100f;
            public float cooldownTime = 0.5f;
            public AudioClip fireSound;

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
            public AudioClip fireSound;

            public float lastFireTime;
            public bool isOnCooldown = false;
        }

        [System.Serializable]
        public class WeaponLandmineProperty
        {
            public bool isActive = false;
            public GameObject bulletPrefab;
            public float cooldownTime = 5f;
            public AudioClip fireSound;

            public float lastFireTime;
            public bool isOnCooldown = false;
        }

        [System.Serializable]
        public class BuffDebuffProperty
        {
            public bool isProectedFromStun;
            public float protectedStunTime;

            public bool isAtkBoosted;
            public float lastAtkBoosted;

            public bool isDefDebuff;
            public float lastDefDebuff;
        }

        #endregion

        #region Variable

        public float spriteScale = 5;
        public float stunSpeedDebuff = 0.4f;
        public float randomSpeedRange = 0.2f;
        public float invulnerableTime = 5;

        public GameObject buffProtectStunBar;
        public GameObject buffAtkBar;
        public GameObject debuffDefBar;
        public GameObject cooldownBar1;
        public GameObject cooldownBar2;

        

        public WeaponGunProperty weaponGun;
        public WeaponSpreadProperty weaponSpread;
        public WeaponLandmineProperty weaponLandmine;

        public BuffDebuffProperty buffDebuff;

        private Rigidbody2D rigidbody;
        private AudioSource audio;
        private float health;
        private float lastHitTime;
        private bool isInvulnerable;

        private Slider buffProtectStunBarSlider;
        private Slider buffAtkBarSlider;
        private Slider debuffDefBarSlider;
        private Slider cooldownBarSlider1;
        private Slider cooldownBarSlider2;



        [Header("New")]
        [Min(0)]
        [SerializeField] float m_InitMoveSpeed = 15;

        [Min(0)]
        [SerializeField] float m_InitSpeedAccel = 15;

        [Min(0)]
        [SerializeField] float m_AccelInstability = 0.6f;
        [Min(0)]
        [SerializeField] float m_MoveSpeedLimit = 7.5f;

        [Header("Stun")]
        [SerializeField] float m_StunDuration = 5f;
        [SerializeField] float m_StunSpeedDebuff = 0.9f;

        public float moveSpeed
        {
            get
            {
                if(m_EntityController != null
                    && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                    && entityManager.TryGetComponentData(entity, out MoveSpeedData moveSpeedData))
                {
                    return moveSpeedData.value;
                }

                return 0;
            }
        }

        public float moveSpeedLimit
        {
            get
            {
                if(m_EntityController != null
                    && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                    && entityManager.TryGetComponentData(entity, out MoveSpeedData moveSpeedData)
                    && moveSpeedData.limit.HasValue)
                {
                    return moveSpeedData.limit.Value;
                }

                return moveSpeed;
            }
        }

        public float2 moveSpeedAccelRange
        {
            get
            {
                if(m_EntityController != null
                    && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                    && entityManager.TryGetComponentData(entity, out MoveSpeedAccelData accelData))
                {
                    return accelData.randomRange;
                }

                return default;
            }
        }

        public bool isStun
        {
            get
            {
                if(m_EntityController != null
                    && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                {
                    return entityManager.HasComponent<StunTag>(entity);
                }

                return false;
            }
        }

        EntityController m_EntityController;

        #endregion

        #region Main

        private void Awake()
        {
            m_EntityController = GetComponent<EntityController>();
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rigidbody = gameObject.GetComponent<Rigidbody2D>();
            audio = gameObject.GetComponent<AudioSource>();
            buffProtectStunBarSlider = buffProtectStunBar.GetComponent<Slider>();
            buffAtkBarSlider = buffAtkBar.GetComponent<Slider>();
            debuffDefBarSlider = debuffDefBar.GetComponent<Slider>();
            cooldownBarSlider1 = cooldownBar1.GetComponent<Slider>();
            cooldownBarSlider2 = cooldownBar2.GetComponent<Slider>();

            InitEntity();
        }

        void InitEntity()
        {
            if(m_EntityController != null
                && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
            {
                entityManager.AddComponentData(entity, new MoveSpeedData
                {
                    value = m_InitMoveSpeed,
                    multiplier = 1f,
                    limit = m_MoveSpeedLimit,
                });

                entityManager.AddComponentData(entity, new MoveSpeedAccelData
                {
                    randomRange = new float2(6f, 24f),
                    value = m_InitSpeedAccel,
                    instability = m_AccelInstability,
                });

                entityManager.AddComponentData(entity, new InputData());
                entityManager.AddComponentData(entity, new MoveData());

                entityManager.AddComponentData(entity, new StunData
                {
                    duration = m_StunDuration,
                    speedDebuff = m_StunSpeedDebuff,
                });
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!isInvulnerable)
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    isInvulnerable = true;
                    StartCoroutine(Flash(new Color(0, 0.4f, 0.4f, 0)));
                    lastHitTime = Time.time;
                    health -= 20;
                }
                else if (other.gameObject.CompareTag("EnemyBullet"))
                {
                    isInvulnerable = true;
                    StartCoroutine(Flash(new Color(0, 0.4f, 0.4f, 0)));
                    lastHitTime = Time.time;
                    Destroy(other.gameObject);
                    health -= 20;
                }
            }

            if (!isStun && !buffDebuff.isProectedFromStun)
            {
                if (other.gameObject.CompareTag("Trash"))
                {
                    if(m_EntityController != null
                        && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                        && entityManager.TryGetComponentData(entity, out StunData stunData))
                    {
                        entityManager.AddComponentData(entity, new StunTag());
                        stunData.lastStunTime = Time.time;
                        entityManager.SetComponentData(entity, stunData);
                    }

                    StartCoroutine(Flash(new Color(0, 0, 0, 0.6f)));
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

            UpdateBuffDebuffStatus();
            UpdateUIElement();
            UpdateInput();
            WeaponControl();
            UpdateSprite();
        }

        private void UpdateBuffDebuffStatus()
        {
            if (Time.time - buffDebuff.protectedStunTime >= 10)
            {
                buffDebuff.isProectedFromStun = false;
            }

            if (Time.time - buffDebuff.lastAtkBoosted >= 10)
            {
                buffDebuff.isAtkBoosted = false;
            }

            if (Time.time - buffDebuff.lastDefDebuff >= 10)
            {
                buffDebuff.isDefDebuff = false;
            }
        }

        private void UpdateUIElement()
        {
            if (weaponSpread.isOnCooldown)
            {
                cooldownBar1.SetActive(true);
                cooldownBarSlider1.value = (weaponSpread.lastFireTime + weaponSpread.cooldownTime - Time.time) /
                                          weaponSpread.cooldownTime;
            }
            else
            {
                cooldownBar1.SetActive(false);
            }

            if (weaponLandmine.isOnCooldown)
            {
                cooldownBar2.SetActive(true);
                cooldownBarSlider2.value = (weaponLandmine.lastFireTime + weaponLandmine.cooldownTime - Time.time) /
                                          weaponLandmine.cooldownTime;
            }
            else
            {
                cooldownBar2.SetActive(false);
            }

            if (buffDebuff.isProectedFromStun)
            {
                buffProtectStunBar.SetActive(true);
                buffProtectStunBarSlider.value = (buffDebuff.protectedStunTime + 10.0f - Time.time) / 10.0f;
            }
            else
            {
                buffProtectStunBar.SetActive(false);
            }

            if (buffDebuff.isAtkBoosted)
            {
                buffAtkBar.SetActive(true);
                buffAtkBarSlider.value = (buffDebuff.lastAtkBoosted + 10.0f - Time.time) / 10.0f;
            }
            else
            {
                buffAtkBar.SetActive(false);
            }

            if (buffDebuff.isDefDebuff)
            {
                debuffDefBar.SetActive(true);
                debuffDefBarSlider.value = (buffDebuff.lastDefDebuff + 10.0f - Time.time) / 10.0f;
            }
            else
            {
                debuffDefBar.SetActive(false);
            }
        }

        void UpdateInput()
        {
            if (m_EntityController == null
                || !m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                || !entityManager.TryGetComponentData(entity, out InputData inputData))
                return;

            inputData.axis.x = Input.GetAxisRaw("Horizontal");
            inputData.axis.y = Input.GetAxisRaw("Vertical");
            inputData.axis = inputData.axis.Normalize();
            entityManager.SetComponentData(entity, inputData);
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

                if (weaponGun.fireSound)
                {
                    audio.PlayOneShot(weaponGun.fireSound);
                }
            }

            // Fire Spread Bullets
            if (weaponSpread.isActive && !weaponSpread.isOnCooldown && Input.GetMouseButtonDown(1))
            {
                for (int i = 0; i < 360; i += weaponSpread.spreadBulletNumber)
                {
                    double angle1 = Math.Cos(DegreeToRadian(i));
                    double angle2 = Math.Sin(DegreeToRadian(i));
                    Vector2 projposition = new Vector2((float)angle1, (float)angle2);

                    GameObject bullet = GameObject.Instantiate(weaponSpread.bulletPrefab, rigidbody.position, Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().linearVelocity = projposition.normalized * weaponSpread.bulletSpeed;
                    Destroy(bullet, 0.2f);
                    weaponSpread.lastFireTime = Time.time;
                    weaponSpread.isOnCooldown = true;
                }

                if (weaponSpread.fireSound)
                {
                    audio.PlayOneShot(weaponSpread.fireSound);
                }
            }

            // Landmine
            if (weaponLandmine.isActive && !weaponLandmine.isOnCooldown && Input.GetMouseButtonDown(2))
            {
                GameObject landmine = GameObject.Instantiate(weaponLandmine.bulletPrefab, rigidbody.position, Quaternion.identity);
                Destroy(landmine, 5f);
                weaponLandmine.lastFireTime = Time.time;
                weaponLandmine.isOnCooldown = true;

                if (weaponLandmine.fireSound)
                {
                    audio.PlayOneShot(weaponLandmine.fireSound);
                }
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
            return Math.PI * (a / 180.0);
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

        private IEnumerator Flash(Color toColor)
        {
            SpriteRenderer playerSprite = gameObject.GetComponent<SpriteRenderer>();

            for (int i = 0; i < invulnerableTime * 10; i++)
            {
                playerSprite.color = new Color(
                    playerSprite.color.r - toColor.r,
                    playerSprite.color.g - toColor.g,
                    playerSprite.color.b - toColor.b,
                    playerSprite.color.a - toColor.a);
                yield return new WaitForSeconds(0.05f);
                playerSprite.color = new Color(
                    playerSprite.color.r + toColor.r,
                    playerSprite.color.g + toColor.g,
                    playerSprite.color.b + toColor.b,
                    playerSprite.color.a + toColor.a); ;
                yield return new WaitForSeconds(0.05f);
            }
        }

        #endregion
    }
}