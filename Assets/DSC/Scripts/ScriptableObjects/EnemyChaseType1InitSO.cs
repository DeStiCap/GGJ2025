using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "EnemyChaseType1InitSO", menuName = "DSC/Entity Init/Enemy Chase Type 1")]
    public class EnemyChaseType1InitSO : EntityInitSO
    {
        public override void Init(Entity entity, EntityManager entityManager)
        {

            entityManager.AddComponentData(entity, new EnemyChaseType1Tag());
        }
    }
}