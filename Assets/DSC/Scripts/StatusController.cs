using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GGJ2025
{
    public class StatusController : MonoBehaviour, IDamageable
    {
        #region Variable

        [SerializeField] float m_InitMaxHp = 10f;

        [Min(0.01f)]
        [SerializeField] float m_DamageIFrameDuration = 0.2f;

        [SerializeField] GameObject m_DamageParticle;
        [SerializeField] bool m_DestroyOnDeath = true;
        [SerializeField] Slider m_HpBar;

        [Header("Events")]
        [SerializeField] UnityEvent<float, float> m_OnHpChanged;
        [SerializeField] UnityEvent<float> m_OnTakeDamageEvent;
        [SerializeField] UnityEvent m_OnDeadEvent;

        public float currentHp
        {
            get
            {
                if (m_EntityController == null
                    || !m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                    return default;

                if (entityManager.TryGetComponentData(entity, out HpData hpData))
                {
                    return hpData.hp;
                }

                return default;
            }
        }

        public float maxHp
        {
            get
            {
                if (m_EntityController == null
                    || !m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                    return default;

                if(entityManager.TryGetComponentData(entity, out HpData hpData))
                {
                    return hpData.maxHp;
                }

                return default;
            }
        }

        bool m_IsDead;

        float m_CanTakeDamageTime;

        EntityController m_EntityController;

        #endregion

        #region Main

        private void Awake()
        {
            m_EntityController = GetComponent<EntityController>();
        }

        private void Start()
        {

            if (m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
            {
                entityManager.AddComponentData(entity, new HpData
                {
                    hp = m_InitMaxHp,
                    maxHp = m_InitMaxHp,
                });


                if (m_HpBar != null)
                {
                    m_HpBar.maxValue = m_InitMaxHp;
                    m_HpBar.value = m_InitMaxHp;
                }
            }
        }

        public void TakeDamage(float damage)
        {
            if (m_IsDead 
                || damage <= 0
                || Time.time < m_CanTakeDamageTime)
                return;


            if (!m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                return;

            var hpData = entityManager.GetComponentData<HpData>(entity);

            

            var previousHp = hpData.hp;

            hpData.hp -= damage;

            m_OnHpChanged?.Invoke(previousHp, hpData.hp);

            if (m_HpBar != null)
            {

                m_HpBar.value = hpData.hp;
            }

            m_CanTakeDamageTime = Time.time + m_DamageIFrameDuration;
            m_OnTakeDamageEvent?.Invoke(damage);

            if(m_DamageParticle != null)
            {
                Instantiate(m_DamageParticle, transform.position, Quaternion.identity);
            }

            if(hpData.hp <= 0)
            {
                hpData.hp = 0;

                if (m_DestroyOnDeath)
                {
                    Destroy(gameObject);
                }

                m_IsDead = true;
                m_OnDeadEvent?.Invoke();
            }

            entityManager.SetComponentData(entity, hpData);
        }

        public void AddOnDeadCallBack(UnityAction callback)
        {
            m_OnDeadEvent?.AddListener(callback);
        }

        public void RemoveOnDeadCallBack(UnityAction callback)
        {
            m_OnDeadEvent?.RemoveListener(callback);
        }

        #endregion
    }
}