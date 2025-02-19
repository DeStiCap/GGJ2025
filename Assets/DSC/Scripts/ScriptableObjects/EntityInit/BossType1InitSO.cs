using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "BossType1InitSO", menuName = "DSC/Entity Init/Boss Type 1")]
    public class BossType1InitSO : EntityInitSO
    {
        [Min(0)]
        [SerializeField] float m_AuraDamagePerHpMax = 5f;
        [Min(0)]
        [SerializeField] float m_AuraDamageInterval = 10f;

        public override void Init(Entity entity, EntityManager entityManager)
        {
            entityManager.AddComponentData(entity, new BossTag());
            entityManager.AddComponentData(entity, new BossType1Data
            {
                auraDamageInterval = m_AuraDamageInterval,
                auraDamagePerHpMax = m_AuraDamagePerHpMax,
                nextAuraDamageTime = Time.time + m_AuraDamageInterval,
                nextAuraDamage = m_AuraDamagePerHpMax,
            });
        }
    }
}