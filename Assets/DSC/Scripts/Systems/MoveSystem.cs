using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial struct MoveSystem : ISystem
    {
        #region Main

        EntityQuery m_Query;

        public void OnCreate(ref SystemState state)
        {
            m_Query = state.GetEntityQuery(
                ComponentType.ReadOnly<MoveData>(), 
                ComponentType.ReadOnly<MoveTag>(),
                ComponentType.ReadOnly<GameObjectData>());
        }

        public void OnUpdate(ref SystemState state)
        {
            if (m_Query.CalculateEntityCount() <= 0)
                return;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach(var (gameObjectData, moveData, entity) in SystemAPI.Query<GameObjectData, RefRO<MoveData>>().WithAll<MoveTag>().WithEntityAccess())
            {
                ecb.RemoveComponent<MoveTag>(entity);

                var rigidbody = gameObjectData.rigidbody;
                if (rigidbody == null)
                    continue;

                Vector2 move = moveData.ValueRO.value;
                move += rigidbody.position;
                rigidbody.MovePosition(move);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        #endregion
    }
}