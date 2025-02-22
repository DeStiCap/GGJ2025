using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct SetEnableTriggerBlindSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            // Disable
            foreach(var (triggerBlindEnable, entity) 
                in SystemAPI.Query<
                    EnabledRefRW<TriggerBlindData>>()
                    .WithAll<TriggerBlindDisableTag>()
                    .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                    .WithEntityAccess())
            {
                triggerBlindEnable.ValueRW = false;
                ecb.RemoveComponent<TriggerBlindDisableTag>(entity);
            }

            // Enable
            foreach (var (triggerBlindEnable, entity)
                in SystemAPI.Query<
                    EnabledRefRW<TriggerBlindData>>()
                    .WithAll<TriggerBlindEnableTag>()
                    .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                    .WithEntityAccess())
            {
                triggerBlindEnable.ValueRW = true;

                ecb.RemoveComponent<TriggerBlindEnableTag>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }

    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    [UpdateAfter(typeof(SetEnableTriggerBlindSystem))]
    [UpdateAfter(typeof(AIMoveEndSystem))]
    public partial struct EndMoveSetEnableTriggerBlindSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);


            // Disable
            foreach (var (triggerBlindEnable, entity) 
                in SystemAPI.Query<
                    EnabledRefRW<TriggerBlindData>>()
                    .WithAll<EndMoveTriggerBlindDisableTag, MoveCooldownTag>()
                    .WithEntityAccess())
            {
                triggerBlindEnable.ValueRW = false;
                ecb.RemoveComponent<EndMoveTriggerBlindDisableTag>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}