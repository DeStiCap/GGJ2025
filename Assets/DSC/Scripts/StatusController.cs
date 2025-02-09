using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GGJ2025
{
    public class StatusController : MonoBehaviour, IDamageable
    {
        #region Variable

        [SerializeField] Status m_Status;

        [Min(0.01f)]
        [SerializeField] float m_DamageIFrameDuration = 0.2f;

        [SerializeField] GameObject m_DamageParticle;
        [SerializeField] bool m_DestroyOnDeath = true;
        [SerializeField] private GameObject m_healthBarUi;

        [Header("Events")]
        [SerializeField] UnityEvent<float, float> m_OnHpChanged;
        [SerializeField] UnityEvent<float> m_OnTakeDamageEvent;
        [SerializeField] UnityEvent m_OnDeadEvent;

        public float currentHp { get { return m_Status.hp; } }
        public float maxHp { get { return m_Status.maxHp; } }

        bool m_IsDead;

        float m_CanTakeDamageTime;
        Slider healthBar;

        #endregion

        #region Main

        private void Awake()
        {
            m_Status.hp = m_Status.maxHp;
            if (m_healthBarUi != null)
            {
                healthBar = m_healthBarUi.GetComponent<Slider>();
            }
        }

        public void TakeDamage(float damage)
        {
            TakeDamage(damage, null);
        }

        public void TakeDamage(float damage, GameObject damageParticle)
        {
            if (m_IsDead
                || Time.time < m_CanTakeDamageTime)
                return;

            var previousHp = m_Status.hp;

            m_Status.hp -= damage;

            m_OnHpChanged?.Invoke(previousHp, m_Status.hp);

            if (healthBar != null)
            {

                healthBar.value = m_Status.hp * (healthBar.maxValue / m_Status.maxHp);
            }

            m_CanTakeDamageTime = Time.time + m_DamageIFrameDuration;
            m_OnTakeDamageEvent?.Invoke(damage);

            var particle = damageParticle != null ? damageParticle : m_DamageParticle;

            if (particle != null)
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }

            if (m_Status.hp <= 0)
            {
                m_Status.hp = 0;

                if (m_DestroyOnDeath)
                {
                    Destroy(gameObject);
                }

                m_IsDead = true;
                m_OnDeadEvent?.Invoke();
            }
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