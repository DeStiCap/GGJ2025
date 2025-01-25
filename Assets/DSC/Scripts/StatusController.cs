using UnityEngine;
using UnityEngine.Events;

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

        [SerializeField] UnityEvent<int> m_OnTakeDamageEvent;
        [SerializeField] UnityEvent m_OnDeathEvent;

        bool m_IsDeath;

        float m_CanTakeDamageTime;

        #endregion

        #region Main

        private void Awake()
        {
            m_Status.hp = m_Status.maxHp;
        }

        public void TakeDamage(int damage)
        {
            if (m_IsDeath 
                || damage <= 0
                || Time.time < m_CanTakeDamageTime)
                return;


            m_Status.hp -= damage;

            m_CanTakeDamageTime = Time.time + m_DamageIFrameDuration;
            m_OnTakeDamageEvent?.Invoke(damage);

            if(m_DamageParticle != null)
            {
                Instantiate(m_DamageParticle, transform.position, Quaternion.identity);
            }

            if(m_Status.hp <= 0)
            {
                m_Status.hp = 0;

                if (m_DestroyOnDeath)
                {
                    Destroy(gameObject);
                }

                m_IsDeath = true;
                m_OnDeathEvent?.Invoke();
            }
        }

        #endregion
    }
}