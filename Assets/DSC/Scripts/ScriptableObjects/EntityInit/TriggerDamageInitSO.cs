using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "TriggerDamageInitSO", menuName = "DSC/Entity Init/Trigger Damage")]
    public class TriggerDamageInitSO : EntityInitSO
    {
        [Min(0)]
        [SerializeField] float m_Damage = 1;

        public override void Init(Entity entity, EntityManager entityManager)
        {
            entityManager.AddComponentData(entity, new TriggerDamageData
            {
                damage = m_Damage,
            });
        }
    }
}