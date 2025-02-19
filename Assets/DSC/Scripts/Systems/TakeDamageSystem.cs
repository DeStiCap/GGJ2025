using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct TakeDamageSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach(var (gameObjectData, takeDamageBuffer) 
                in SystemAPI.Query<GameObjectData, DynamicBuffer<TakeDamageBuffer>>())
            {
                if (takeDamageBuffer.Length <= 0
                    || gameObjectData.gameObject == null
                    || !gameObjectData.gameObject.TryGetComponent(out IDamageable damageable))
                    continue;

                foreach (var takeDamage in takeDamageBuffer)
                {
                    damageable.TakeDamage(takeDamage.damage);
                }

                takeDamageBuffer.Clear();
            }
        }
    }
}