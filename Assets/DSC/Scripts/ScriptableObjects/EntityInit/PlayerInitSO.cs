using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "PlayerInitSO", menuName = "DSC/Entity Init/Player")]
    public class PlayerInitSO : EntityInitSO
    {
        public override void Init(EntityMB entityMB, Entity entity, EntityManager entityManager)
        {
            entityManager.AddComponentData(entity, new PlayerTag());
            entityManager.AddComponentData(entity, new FactionData
            {
                id = 0,
            });
            entityManager.AddBuffer<TakeDamageBuffer>(entity);

        }
    }
}