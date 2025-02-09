using UnityEngine;

namespace GGJ2025
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        void TakeDamage(float damage, GameObject damageParticle);
    }
}