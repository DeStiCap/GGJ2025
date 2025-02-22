using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct EnemyChaseType2InitSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float time = Time.time;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach(var (
                moveDirectionData, moveTimeData, 
                positionData, targetData, entity) 
                in SystemAPI.Query<
                    RefRW<MoveDirectionData>, RefRW<MoveTimeData>,
                    RefRO<PositionData>, RefRO<TargetData>>()
                    .WithAll<EnemyChaseType2Tag, ChaseTag, AIMoveStartTag>()
                    .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                    .WithEntityAccess())
            {
                var targetEntity = targetData.ValueRO.value;
                if (targetEntity == Entity.Null
                    || !SystemAPI.HasComponent<PositionData>(targetEntity))
                    continue;

                var targetPosition = SystemAPI.GetComponent<PositionData>(targetEntity).value;

                moveDirectionData.ValueRW.value = (targetPosition - positionData.ValueRO.value).Normalize();
                moveTimeData.ValueRW.startTime = time;
                moveTimeData.ValueRW.endTime = time + 1.5f;

                ecb.RemoveComponent<AIMoveStartTag>(entity);
                ecb.AddComponent<AIMoveTag>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}