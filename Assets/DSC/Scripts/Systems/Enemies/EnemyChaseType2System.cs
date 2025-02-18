using Unity.Burst;
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
            float time = Time.time;
            float fixedDeltaTime = Time.fixedDeltaTime;


            foreach(var (
                moveData, moveDirectionData, 
                moveTimeData, chargeAttackData, positionData, targetData, entity) 
                in SystemAPI.Query<
                    RefRW<MoveData>, RefRW<MoveDirectionData>, 
                    RefRO<MoveTimeData>, RefRO<ChargeAttackData>, RefRO<PositionData>, RefRO<TargetData>>()
                .WithAll<AIMoveTag, ChaseTag>()
                .WithNone<AIMoveStartTag>()
                .WithEntityAccess())
            {
                var targetEntity = targetData.ValueRO.value;
                if (time < moveTimeData.ValueRO.startTime + chargeAttackData.ValueRO.chargeDuration)
                {
                    if (targetEntity != Entity.Null
                        && SystemAPI.HasComponent<PositionData>(targetEntity))
                    {
                        var targetPosition = SystemAPI.GetComponent<PositionData>(targetEntity).value;
                        moveDirectionData.ValueRW.value = (targetPosition - positionData.ValueRO.value).Normalize();
                    }

                    continue;
                }

                moveData.ValueRW.value = moveDirectionData.ValueRO.value * chargeAttackData.ValueRO.chargeSpeed * fixedDeltaTime;
            }

        }
    }
}