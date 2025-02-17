using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(AIUpdateSystemGroup))]
    public partial struct EnemyPatrolType1System : ISystem
    {
        #region Main

        EntityQuery m_Query;

        public void OnCreate(ref SystemState state)
        {
            m_Query = state.GetEntityQuery(
                ComponentType.ReadOnly<EnemyPatrolType1Tag>(),
                ComponentType.ReadOnly<AIMoveTag>(),

                ComponentType.ReadOnly<AIStateData>(),
                ComponentType.ReadOnly<MoveDirectionData>(),
                ComponentType.ReadOnly<MoveSpeedData>(),
                ComponentType.ReadOnly<MoveTimeData>(),
                ComponentType.ReadOnly<MoveCurveData>(),
                ComponentType.ReadOnly<GameObjectData>(),

                ComponentType.ReadWrite<MoveData>(),
                ComponentType.Exclude<AIMoveStartTag>(),

                ComponentType.ReadWrite<TargetData>(),
                ComponentType.ReadWrite<DetectData>());;
        }

        public void OnUpdate(ref SystemState state)
        {
            if (m_Query.CalculateEntityCount() <= 0)
                return;


            float time = Time.time;
            float fixedDeltaTime = Time.fixedDeltaTime;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach(var (gameObjectData, moveCurveData, moveData, aiState, moveDirectionData, moveSpeedData, moveTimeData, entity)
                in SystemAPI.Query<GameObjectData, MoveCurveData, RefRW<MoveData>, RefRO<AIStateData>, RefRO<MoveDirectionData>, RefRO<MoveSpeedData>, RefRO<MoveTimeData>>()
                .WithAll<EnemyPatrolType1Tag, AIMoveTag>()
                .WithNone<AIMoveStartTag>()
                .WithEntityAccess())
            {
                if (aiState.ValueRO.value != AIState.Patrol)
                    continue;

                if(moveCurveData.value != null)
                {
                    moveData.ValueRW.value = moveDirectionData.ValueRO.value * moveCurveData.value.Evaluate(time - moveTimeData.ValueRO.startTime) * moveSpeedData.ValueRO.value * fixedDeltaTime;


                    ecb.AddComponent(entity, new MoveTag());
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        #endregion
    }
}