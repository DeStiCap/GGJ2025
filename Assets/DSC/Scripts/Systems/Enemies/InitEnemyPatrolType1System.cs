using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace GGJ2025
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct InitEnemyPatrolType1System : ISystem
    {
        #region Main

        EntityQuery m_Query;

        ComponentLookup<PositionData> m_LookupPositionData;
        ComponentLookup<AreaRangeData> m_LookupAreaRangeData;

        public void OnCreate(ref SystemState state)
        {
            m_Query = state.GetEntityQuery(
                ComponentType.ReadOnly<EnemyPatrolType1Tag>(), 
                ComponentType.ReadOnly<AIMoveStartTag>(),
                ComponentType.ReadOnly<AIGroupData>(),
                ComponentType.ReadOnly<PositionData>(),
                ComponentType.ReadOnly<AIStateData>(),
                ComponentType.ReadOnly<MoveCurveData>(),
                ComponentType.ReadWrite<MoveDirectionData>(),
                ComponentType.ReadWrite<MoveTimeData>(),
                
                ComponentType.Exclude<MoveCooldownTag>());

            m_LookupPositionData = state.GetComponentLookup<PositionData>(true);
            m_LookupAreaRangeData = state.GetComponentLookup<AreaRangeData>(true);
        }

        public void OnUpdate(ref SystemState state)
        {
            if (m_Query.CalculateEntityCount() <= 0)
                return;

            m_LookupPositionData.Update(ref state);
            m_LookupAreaRangeData.Update(ref state);

            var random = new Unity.Mathematics.Random((uint)DateTime.UtcNow.Ticks & 0x00000000FFFFFFFF);

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach(var (moveCurveData, moveDirectionData, moveTimeData, aiGroupData, positionData, aiStateData, entity)
                in SystemAPI.Query<MoveCurveData, RefRW<MoveDirectionData>, RefRW<MoveTimeData>, RefRO<AIGroupData>, RefRO<PositionData>, RefRO<AIStateData>>()
                .WithAll<EnemyPatrolType1Tag, AIMoveStartTag>()
                .WithNone<MoveCooldownTag>()
                .WithEntityAccess())
            {
                if (aiStateData.ValueRO.value != AIState.Patrol)
                    continue;

                ecb.RemoveComponent<AIMoveStartTag>(entity);
                ecb.AddComponent<AIMoveTag>(entity);

                var position = positionData.ValueRO.value;
                var aiGroupEntity = aiGroupData.ValueRO.groupEntity;

                var direction = (random.NextFloat2(new float2(-1, -1), new float2(1, 1))).Normalize();

                if (m_LookupPositionData.HasComponent(aiGroupEntity)
                    && m_LookupAreaRangeData.HasComponent(aiGroupEntity))
                {
                    var groupPosition = m_LookupPositionData.GetRefRO(aiGroupEntity).ValueRO.value;
                    var groupAreaRange = m_LookupAreaRangeData.GetRefRO(aiGroupEntity).ValueRO.value;


                    var limitX = new float2(groupPosition.x - groupAreaRange.x, groupPosition.x + groupAreaRange.x);
                    var limitY = new float2(groupPosition.y - groupAreaRange.y, groupPosition.y + groupAreaRange.y);
                    if (position.x < limitX.x
                    || position.x > limitX.y
                        || position.y < limitY.x
                        || position.y > limitY.y)
                    {
                        direction = (groupPosition - position).Normalize();
                    }
                }

                moveDirectionData.ValueRW.value = direction;


                if(moveCurveData.value != null)
                {
                    moveTimeData.ValueRW.startTime = Time.time;
                    moveTimeData.ValueRW.endTime = Time.time + moveCurveData.value.keys[moveCurveData.value.length - 1].time;
                }
            }


            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        #endregion
    }
}