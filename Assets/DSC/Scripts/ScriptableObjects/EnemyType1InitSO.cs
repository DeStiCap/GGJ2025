using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "EnemyType1InitSO", menuName = "DSC/Entity Init/Enemy Type 1")]
    public class EnemyType1InitSO : EntityInitSO
    {
        [SerializeField] AnimationCurve m_PatrolMoveCurve;

        public override void Init(Entity entity, EntityManager entityManager)
        {
            entityManager.AddComponentData(entity, new EnemyPatrolType1Tag());
            entityManager.AddComponentData(entity, new EnemyChaseType1Tag());
            entityManager.AddComponentData(entity, new AIMoveStartTag());
            entityManager.AddComponentData(entity, new MoveTimeData());
            entityManager.AddComponentData(entity, new MoveCooldownData());
            entityManager.AddComponentObject(entity, new MoveCurveData
            {
                value = m_PatrolMoveCurve
            });
        }
    }
}