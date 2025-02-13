using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct DestroyEndFrameSystem : ISystem
    {
        #region Main

        EntityQuery m_Query;

        public void OnCreate(ref SystemState state)
        {
            m_Query = state.GetEntityQuery(ComponentType.ReadOnly<DestroyEndFrameTag>());
        }


        public void OnUpdate(ref SystemState state)
        {
            if (m_Query.CalculateEntityCount() <= 0)
                return;

            var entities = m_Query.ToEntityArray(Allocator.Temp);
            
            state.EntityManager.DestroyEntity(entities);

            entities.Dispose();
        }

        #endregion
    }
}