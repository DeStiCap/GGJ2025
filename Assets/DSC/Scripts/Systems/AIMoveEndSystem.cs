using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct AIMoveEndSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var randomData = SystemAPI.GetSingleton<RandomData>();

            float time = Time.time;

            var random = randomData.randomArr[0];

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach(var (moveCooldownData, moveTimeData, entity) 
                in SystemAPI.Query<
                    RefRW<MoveCooldownData>,
                    RefRO<MoveTimeData>>()
                    .WithAll<AIMoveTag>()
                    .WithNone<MoveCooldownTag>()
                    .WithEntityAccess())
            {
                if (time < moveTimeData.ValueRO.endTime)
                    continue;

                ecb.RemoveComponent<AIMoveTag>(entity);
                ecb.AddComponent(entity, new MoveCooldownTag());

                moveCooldownData.ValueRW.endTime = time + random.NextFloat(0.25f, 1);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}