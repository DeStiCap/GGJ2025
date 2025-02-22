using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    [UpdateAfter(typeof(SetEnableTriggerDamageSystem))]
    [UpdateAfter(typeof(AIMoveEndSystem))]
    public partial struct EndMoveSetEnableTriggerDamageSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            // Enable
            foreach(var (triggerDamageEnable, entity)
                in SystemAPI.Query<
                    EnabledRefRW<TriggerDamageData>>()
                    .WithAll<EndMoveTriggerDamageEnableTag, MoveCooldownTag>()
                    .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                    .WithEntityAccess())
            {
                triggerDamageEnable.ValueRW = true;
                ecb.RemoveComponent<EndMoveTriggerDamageEnableTag>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}