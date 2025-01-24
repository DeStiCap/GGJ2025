using UnityEngine;

namespace GGJ2025
{
    public class StatusController : MonoBehaviour, IDamageable
    {
        #region Variable

        [SerializeField] Status m_Status;

        #endregion

        #region Main

        private void Awake()
        {
            m_Status.hp = m_Status.maxHp;
        }

        public void TakeDamage(int damage)
        {
            m_Status.hp -= damage;

            if(m_Status.hp <= 0)
            {
                m_Status.hp = 0;
                
                Destroy(gameObject);
            }
        }

        #endregion
    }
}