using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(UpdateSystemGroup))]
    public partial struct AIChaseGiveUpSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            EntityQuery query = SystemAPI.QueryBuilder()
                .WithAllRW<TargetData, AIStateData>()
                .WithAll<AIMoveTag, PositionData, GiveUpRangeData>()
                .Build();

            if (query.IsEmpty)
                return;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entities = query.ToEntityArray(Allocator.Temp);

            var targetDatas = query.ToComponentDataArray<TargetData>(Allocator.Temp);
            var aiStateDatas = query.ToComponentDataArray<AIStateData>(Allocator.Temp);
            
            var positionDatas = query.ToComponentDataArray<PositionData>(Allocator.Temp);            
            var giveUpRangeDatas = query.ToComponentDataArray<GiveUpRangeData>(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var positionData = positionDatas[i];
                var targetData = targetDatas[i];
                var targetEntity = targetData.value;
                var aiStateData = aiStateDatas[i];
                var giveUpRange = giveUpRangeDatas[i].value;

                if (targetEntity == Entity.Null
                    || !SystemAPI.HasComponent<PositionData>(targetEntity))
                    continue;

                var targetPosition = SystemAPI.GetComponentRO<PositionData>(targetEntity);

                var distanceSq = math.distancesq(positionData.value, targetPosition.ValueRO.value);

                // Give up!!
                if(distanceSq > math.mul(giveUpRange, giveUpRange))
                {
                    targetData.value = Entity.Null;
                    aiStateData.nextValue = AIState.Patrol;

                    ecb.SetComponent(entity, targetData);
                    ecb.SetComponent(entity, aiStateData);
                    ecb.RemoveComponent<AIMoveTag>(entity);
                    ecb.AddComponent(entity, new MoveCooldownTag());
                }

            }
            

            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            entities.Dispose();
            targetDatas.Dispose();
            aiStateDatas.Dispose();
            positionDatas.Dispose();            
            giveUpRangeDatas.Dispose();
        }
    }
}