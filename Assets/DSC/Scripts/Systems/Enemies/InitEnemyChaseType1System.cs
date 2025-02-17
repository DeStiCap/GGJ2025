using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct InitEnemyChaseType1System : ISystem
    {
        #region Main

        EntityQuery m_Query;

        public void OnCreate(ref SystemState state)
        {
            m_Query = state.GetEntityQuery(
                ComponentType.ReadOnly<EnemyChaseType1Tag>(),
                ComponentType.ReadOnly<AIMoveStartTag>(),
                ComponentType.ReadOnly<AIStateData>(),
                ComponentType.ReadWrite<MoveTimeData>(),
                
                ComponentType.Exclude<MoveCooldownTag>());
        }

        public void OnUpdate(ref SystemState state)
        {
            if (m_Query.CalculateEntityCount() <= 0)
                return;

            float time = Time.time;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entities = m_Query.ToEntityArray(Allocator.Temp);
            var moveTimeDatas = m_Query.ToComponentDataArray<MoveTimeData>(Allocator.Temp);
            var aiStateDatas = m_Query.ToComponentDataArray<AIStateData>(Allocator.Temp);

            for(int i = 0; i < entities.Length; i++)
            {
                if (aiStateDatas[i].value != AIState.Chase)
                    continue;

                var entity = entities[i];
                var moveTimeData = moveTimeDatas[i];

                moveTimeData.startTime = time;
                moveTimeData.endTime = time + 3f;

                ecb.RemoveComponent<AIMoveStartTag>(entity);
                ecb.AddComponent<AIMoveTag>(entity);
                ecb.SetComponent(entity, moveTimeData);
            }


            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            entities.Dispose();
            moveTimeDatas.Dispose();
            aiStateDatas.Dispose();
        }

        #endregion
    }
}