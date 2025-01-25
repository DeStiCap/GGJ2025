using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    public class StatusController : MonoBehaviour, IDamageable
    {
        #region Variable

        [SerializeField] Status m_Status;

        [SerializeField] GameObject m_DamageParticle;

        [SerializeField] UnityEvent<int> m_OnTakeDamageEvent;
        [SerializeField] UnityEvent m_OnDeathEvent;

        #endregion

        #region Main

        private void Awake()
        {
            m_Status.hp = m_Status.maxHp;
        }

        public void TakeDamage(int damage)
        {
            m_Status.hp -= damage;

            m_OnTakeDamageEvent?.Invoke(damage);

            if(m_DamageParticle != null)
            {
                Instantiate(m_DamageParticle, transform.position, Quaternion.identity);
            }

            if(m_Status.hp <= 0)
            {
                m_Status.hp = 0;
                
                Destroy(gameObject);

                m_OnDeathEvent?.Invoke();
            }
        }

        #endregion
    }
}