using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace GGJ2025
{
    [BurstCompile]
    public partial struct ChangeAIStateByTargetSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {

            var ecb = new EntityCommandBuffer(Allocator.Temp);


            foreach(var (aiStateData, targetData, entity) 
                in SystemAPI.Query<RefRW<AIStateData>, RefRO<TargetData>>()
                .WithNone<LockAIStateTag>()
                .WithEntityAccess())
            {
                switch (aiStateData.ValueRO.value)
                {
                    case AIState.Patrol:
                        if(targetData.ValueRO.value != Entity.Null)
                        {
                            aiStateData.ValueRW.nextValue = AIState.Chase;
                            ecb.AddComponent(entity, new AIMoveStartTag());
                        }
                        break;
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();


        }
    }
}