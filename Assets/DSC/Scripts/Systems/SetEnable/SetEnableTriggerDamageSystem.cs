using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct SetEnableTriggerDamageSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (triggerDamageEnable, entity)
                in SystemAPI.Query<
                    EnabledRefRW<TriggerDamageData>>()
                    .WithAll<TriggerDamageDisableTag>()
                    .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                    .WithEntityAccess())
            {
                triggerDamageEnable.ValueRW = false;
                ecb.RemoveComponent<TriggerDamageDisableTag>(entity);
            }

            foreach (var (triggerDamageEnable, entity) 
                in SystemAPI.Query<
                    EnabledRefRW<TriggerDamageData>>()
                    .WithAll<TriggerDamageEnableTag>()
                    .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                    .WithEntityAccess())
            {
                triggerDamageEnable.ValueRW = true;
                ecb.RemoveComponent<TriggerDamageEnableTag>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}