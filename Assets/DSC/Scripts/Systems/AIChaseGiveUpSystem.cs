using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(UpdateSystemGroup))]
    public partial struct AIChaseGiveUpSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (
                aiStateData, targetData, 
                positionData, giveUpRangeData, entity)
                in SystemAPI.Query<
                    RefRW<AIStateData>, RefRW<TargetData>,
                    RefRO<PositionData>, RefRO<GiveUpRangeData>>()
                    .WithEntityAccess())
            {
                var targetEntity = targetData.ValueRO.value;

                if (targetEntity == Entity.Null
                    || !SystemAPI.HasComponent<PositionData>(targetEntity))
                    continue;

                var targetPosition = SystemAPI.GetComponentRO<PositionData>(targetEntity);
                var giveUpRange = giveUpRangeData.ValueRO.value;
                var distanceSq = math.distancesq(positionData.ValueRO.value, targetPosition.ValueRO.value);

                // Give up!!
                if (distanceSq > math.mul(giveUpRange, giveUpRange))
                {
                    targetData.ValueRW.value = Entity.Null;
                    aiStateData.ValueRW.nextValue = AIState.Patrol;

                    ecb.RemoveComponent<AIMoveTag>(entity);
                    ecb.AddComponent(entity, new MoveCooldownTag());
                }

            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}