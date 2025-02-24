using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "EnemyChaseType2InitSO", menuName = "DSC/Entity Init/Enemy Chase Type 2")]
    public class EnemyChaseType2InitSO : EntityInitSO
    {
        public override void Init(EntityMB entityMB, Entity entity, EntityManager entityManager)
        {
            entityManager.AddComponentData(entity, new EnemyChaseType2Tag());

            entityManager.AddComponentData(entity, new ChargeAttackData
            {
                chargeDuration = 1f,
                chargeSpeed = 30f,
            });

            entityManager.AddComponentData(entity, new TriggerBlindData
            {
                duration = 2f,
            });

            entityManager.SetComponentEnabled<TriggerBlindData>(entity, false);
        }
    }
}