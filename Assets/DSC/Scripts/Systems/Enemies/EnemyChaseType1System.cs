using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(AIUpdateSystemGroup))]
    public partial struct EnemyChaseType1System : ISystem
    {
        #region Main

        EntityQuery m_Query;

        public void OnCreate(ref SystemState state)
        {
            m_Query = state.GetEntityQuery(
                ComponentType.ReadOnly<EnemyChaseType1Tag>(),
                ComponentType.ReadOnly<AIMoveTag>(),

                ComponentType.ReadOnly<PositionData>(),
                ComponentType.ReadOnly<MoveSpeedData>(),
                ComponentType.ReadOnly<TargetData>(),
                ComponentType.ReadWrite<MoveData>(),

                
                ComponentType.ReadOnly<GameObjectData>());
        }

        public void OnUpdate(ref SystemState state)
        {
            if (m_Query.CalculateEntityCount() <= 0)
                return;

            float time = Time.time;
            var deltaTime = Time.fixedDeltaTime;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach(var (gameObjectData, moveData, moveSpeedData, targetData, positionData, entity) 
                in SystemAPI.Query<GameObjectData, RefRW<MoveData>, RefRO<MoveSpeedData>, RefRO<TargetData>, RefRO<PositionData>>()
                .WithAll<EnemyChaseType1Tag, AIMoveTag>()
                .WithEntityAccess())
            {
                var targetEntity = targetData.ValueRO.value;
                if (targetEntity == Entity.Null
                    || !SystemAPI.HasComponent<PositionData>(targetEntity))
                    continue;

                var targetPosition = SystemAPI.GetComponentRO<PositionData>(targetEntity).ValueRO.value;
                var direction = (targetPosition - positionData.ValueRO.value).Normalize();
                moveData.ValueRW.value = direction * moveSpeedData.ValueRO.value * deltaTime;

                ecb.AddComponent<MoveTag>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        #endregion
    }
}