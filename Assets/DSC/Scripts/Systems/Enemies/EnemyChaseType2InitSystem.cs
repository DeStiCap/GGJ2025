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
            EntityQuery query = SystemAPI.QueryBuilder()
                .WithAllRW<MoveDirectionData, MoveTimeData>()
                .WithAll<AIStateData, PositionData, TargetData>()
                .WithAll<EnemyChaseType2Tag, AIMoveStartTag>()
                .Build();

            if (query.IsEmpty)
                return;

            float time = Time.time;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entities = query.ToEntityArray(Allocator.Temp);
            var aiStateDatas = query.ToComponentDataArray<AIStateData>(Allocator.Temp);
            var moveDirectionDatas = query.ToComponentDataArray<MoveDirectionData>(Allocator.Temp);
            var moveTimeDatas = query.ToComponentDataArray<MoveTimeData>(Allocator.Temp);
            var positionDatas = query.ToComponentDataArray<PositionData>(Allocator.Temp);
            var targetDatas = query.ToComponentDataArray<TargetData>(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var aiState = aiStateDatas[i].value;
                var moveDiretionData = moveDirectionDatas[i];
                var moveTimeData = moveTimeDatas[i];
                var position = positionDatas[i].value;
                var targetEntity = targetDatas[i].value;

                if(aiState != AIState.Chase
                    || targetEntity == Entity.Null
                    || !SystemAPI.HasComponent<PositionData>(targetEntity))
                    continue;
                
                var targetPosition = SystemAPI.GetComponent<PositionData>(targetEntity).value;

                moveDiretionData.value = (targetPosition - position).Normalize();
                moveTimeData.startTime = time;
                moveTimeData.endTime = time + 1.5f;

                ecb.RemoveComponent<AIMoveStartTag>(entity);
                ecb.AddComponent<AIMoveTag>(entity);
                ecb.SetComponent(entity, moveDiretionData);
                ecb.SetComponent(entity, moveTimeData);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            entities.Dispose();
            aiStateDatas.Dispose();
            moveDirectionDatas.Dispose();
            moveTimeDatas.Dispose();
            positionDatas.Dispose();
            targetDatas.Dispose();
        }
    }
}