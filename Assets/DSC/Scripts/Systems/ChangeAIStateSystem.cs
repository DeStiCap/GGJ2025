using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct ChangeAIStateSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach(var (aiStateData, entity) 
                in SystemAPI.Query<RefRW<AIStateData>>()
                .WithEntityAccess())
            {
                var nextValue = aiStateData.ValueRO.nextValue;
                if (aiStateData.ValueRO.value == nextValue)
                    continue;

                ecb.RemoveComponent<PatrolTag>(entity);
                ecb.RemoveComponent<ChaseTag>(entity);

                switch (nextValue)
                {
                    case AIState.Patrol:
                        ecb.AddComponent<PatrolTag>(entity);
                        break;

                    case AIState.Chase:
                        ecb.AddComponent<ChaseTag>(entity);
                        break;
                }

                aiStateData.ValueRW.value = nextValue;
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}