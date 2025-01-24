using UnityEngine;

namespace GGJ2025
{
    public class DamageController : MonoBehaviour
    {
        #region Enum

        enum FactionType
        {
            Player,
            Enemy,
        }

        #endregion

        #region Variable

        [Min(0)]
        [SerializeField] int m_Damage = 1;
        [SerializeField] FactionType ownerFactionType;

        #endregion

        #region Main

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (ownerFactionType)
            {
                case FactionType.Player:
                    
                    // Ignore Player
                    if (collision.CompareTag("Player"))
                        return;

                    if(collision.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(m_Damage);
                        Destroy(gameObject);
                    }

                    break;
            }
            

        }

        #endregion
    }
}