using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] float m_Damage = 1;
        [SerializeField] FactionType ownerFactionType;
        [SerializeField] bool m_DestroyAfterDoDamage = true;
        [SerializeField] UnityEvent<float> m_OnDoDamageEvent;

        [SerializeField] AudioClip onHitSoundEffect;

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
                        if (onHitSoundEffect)
                        {
                            AudioSource.PlayClipAtPoint(onHitSoundEffect, transform.position);
                        }
                        
                        damageable.TakeDamage(m_Damage);

                        m_OnDoDamageEvent?.Invoke(m_Damage);

                        if (m_DestroyAfterDoDamage)
                        {
                            Destroy(gameObject);
                        }
                    }

                    break;
            }
            

        }

        public void SetDamage(float damage)
        {
            m_Damage = damage;
        }

        #endregion
    }
}