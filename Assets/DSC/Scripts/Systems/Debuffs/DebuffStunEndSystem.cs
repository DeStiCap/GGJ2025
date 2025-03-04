using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct DebuffStunEndSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float time = Time.time;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach(var (stunData, entity) 
                in SystemAPI.Query<RefRO<StunData>>()
                .WithAll<StunTag>()
                .WithEntityAccess())
            {
                if (time < stunData.ValueRO.lastStunTime + stunData.ValueRO.duration)
                    continue;

                ecb.RemoveComponent<StunTag>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}