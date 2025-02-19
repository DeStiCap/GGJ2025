using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "EnemyPatrolType1InitSO", menuName = "DSC/Entity Init/Enemy Patrol Type 1")]
    public class EnemyPatrolType1InitSO : EntityInitSO
    {
        [SerializeField] AnimationCurve m_PatrolMoveCurve;

        public override void Init(Entity entity, EntityManager entityManager)
        {
            entityManager.AddComponentData(entity, new EnemyPatrolType1Tag());

            entityManager.AddComponentObject(entity, new MoveCurveData
            {
                value = m_PatrolMoveCurve
            });
        }
    }
}