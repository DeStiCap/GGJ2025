using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(AIUpdateSystemGroup))]
    public partial struct EnemyChaseType2System : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            EntityQuery query = SystemAPI.QueryBuilder()
                .WithAllRW<MoveData, MoveDirectionData>()
                .WithAll<AIStateData, ChargeAttackData, MoveTimeData, PositionData, TargetData>()
                .WithAll<AIMoveTag>()
                .WithNone<AIMoveStartTag>()
                .Build();

            if (query.IsEmpty)
                return;

            float time = Time.time;
            float fixedDeltaTime = Time.fixedDeltaTime;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entities = query.ToEntityArray(Allocator.Temp);
            var moveDatas = query.ToComponentDataArray<MoveData>(Allocator.Temp);
            var moveDirectionDatas = query.ToComponentDataArray<MoveDirectionData>(Allocator.Temp);
            var aiStateDatas = query.ToComponentDataArray<AIStateData>(Allocator.Temp);
            var chargeAttackDatas = query.ToComponentDataArray<ChargeAttackData>(Allocator.Temp);
            var moveTimeDatas = query.ToComponentDataArray<MoveTimeData>(Allocator.Temp);
            var positionDatas = query.ToComponentDataArray<PositionData>(Allocator.Temp);
            var targetDatas = query.ToComponentDataArray<TargetData>(Allocator.Temp);


            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var aiState = aiStateDatas[i].value;
                var moveData = moveDatas[i];
                var moveDirectionData = moveDirectionDatas[i];
                var chargeAttackData = chargeAttackDatas[i];
                var moveTimeData = moveTimeDatas[i];
                var position = positionDatas[i].value;
                var targetEntity = targetDatas[i].value;

                if (aiState != AIState.Chase)
                    continue;

                if (time < moveTimeData.startTime + chargeAttackData.chargeDuration)
                {
                    if(targetEntity != Entity.Null
                        && SystemAPI.HasComponent<PositionData>(targetEntity))
                    {
                        var targetPosition = SystemAPI.GetComponent<PositionData>(targetEntity).value;
                        moveDirectionData.value = (targetPosition - position).Normalize();
                        ecb.SetComponent(entity,moveDirectionData);
                    }

                    continue;
                }

                moveData.value = moveDirectionData.value * chargeAttackData.chargeSpeed * fixedDeltaTime;
                ecb.SetComponent(entity, moveData);
            }


            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            entities.Dispose();
            moveDatas.Dispose();
            moveDirectionDatas.Dispose();
            aiStateDatas.Dispose();
            chargeAttackDatas.Dispose();
            moveTimeDatas.Dispose();
            positionDatas.Dispose();
            targetDatas.Dispose();
        }
    }
}