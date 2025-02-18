using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(AIUpdateSystemGroup))]
    public partial struct EnemyChaseType1System : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float time = Time.time;
            var deltaTime = Time.fixedDeltaTime;

            foreach(var (
                moveData, 
                moveSpeedData, targetData, positionData, entity) 
                in SystemAPI.Query<
                    RefRW<MoveData>, 
                    RefRO<MoveSpeedData>, RefRO<TargetData>, RefRO<PositionData>>()
                .WithAll<EnemyChaseType1Tag, AIMoveTag, ChaseTag>()
                .WithEntityAccess())
            {
                var targetEntity = targetData.ValueRO.value;
                if (targetEntity == Entity.Null
                    || !SystemAPI.HasComponent<PositionData>(targetEntity))
                    continue;

                var targetPosition = SystemAPI.GetComponentRO<PositionData>(targetEntity).ValueRO.value;
                var direction = (targetPosition - positionData.ValueRO.value).Normalize();
                moveData.ValueRW.value = direction * moveSpeedData.ValueRO.value * deltaTime;
            }

        }

    }
}