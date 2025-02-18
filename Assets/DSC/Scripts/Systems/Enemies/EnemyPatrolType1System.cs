using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(AIUpdateSystemGroup))]
    public partial struct EnemyPatrolType1System : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {

            float time = Time.time;
            float fixedDeltaTime = Time.fixedDeltaTime;

            foreach(var (
                moveCurveData, moveData, 
                moveDirectionData, moveSpeedData, moveTimeData, entity)
                in SystemAPI.Query<
                    MoveCurveData, RefRW<MoveData>, 
                    RefRO<MoveDirectionData>, RefRO<MoveSpeedData>, RefRO<MoveTimeData>>()
                .WithAll<EnemyPatrolType1Tag, AIMoveTag, PatrolTag>()
                .WithNone<AIMoveStartTag>()
                .WithEntityAccess())
            {
                if(moveCurveData.value != null)
                {
                    moveData.ValueRW.value = moveDirectionData.ValueRO.value * moveCurveData.value.Evaluate(time - moveTimeData.ValueRO.startTime) * moveSpeedData.ValueRO.value * fixedDeltaTime;
                }
            }
        }
    }
}