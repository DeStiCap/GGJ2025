using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "AIBaseInitSO", menuName = "DSC/Entity Init/AI Base")]
    public class AIBaseInitSO : EntityInitSO
    {
        public override void Init(Entity entity, EntityManager entityManager)
        {
            entityManager.AddComponentData(entity, new AIMoveStartTag());
            entityManager.AddComponentData(entity, new MoveTimeData());
            entityManager.AddComponentData(entity, new MoveCooldownData());
        }
    }
}