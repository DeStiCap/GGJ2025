using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "BossType1InitSO", menuName = "DSC/Entity Init/Boss Type 1")]
    public class BossType1InitSO : EntityInitSO
    {
        [SerializeField] EnemySpawnerController m_GroupSpawner;
        [SerializeField] GameObject m_AuraParticlePrefab;

        [Min(0)]
        [SerializeField] float m_AuraDamagePerHpMax = 5f;
        [Min(0)]
        [SerializeField] float m_AuraDamageInterval = 10f;

        public override void Init(EntityMB entityMB, Entity entity, EntityManager entityManager)
        {
            entityManager.AddComponentData(entity, new BossTag());
            entityManager.AddComponentData(entity, new BossType1Data
            {
                auraDamageInterval = m_AuraDamageInterval,
                auraDamagePerHpMax = m_AuraDamagePerHpMax,
                nextAuraDamageTime = Time.time + m_AuraDamageInterval,
                nextAuraDamage = m_AuraDamagePerHpMax,
            });

            // Test only
            Instantiate(m_AuraParticlePrefab, entityMB.transform);

            if (m_GroupSpawner)
            {
                Instantiate(m_GroupSpawner, entityMB.transform);
            }
        }
    }
}