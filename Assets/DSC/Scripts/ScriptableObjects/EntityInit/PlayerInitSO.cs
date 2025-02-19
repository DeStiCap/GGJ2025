using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "PlayerInitSO", menuName = "DSC/Entity Init/Player")]
    public class PlayerInitSO : EntityInitSO
    {
        public override void Init(Entity entity, EntityManager entityManager)
        {
            entityManager.AddComponentData(entity, new PlayerTag());
            entityManager.AddBuffer<TakeDamageBuffer>(entity);

        }
    }
}