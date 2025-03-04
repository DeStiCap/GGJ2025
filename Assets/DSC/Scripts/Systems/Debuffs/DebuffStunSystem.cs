using Unity.Burst;
using Unity.Entities;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(PreFixedUpdateSystemGroup))]
    public partial struct DebuffStunSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach(var (moveSpeedData, stunData) 
                in SystemAPI.Query<
                    RefRW<MoveSpeedData>,
                    RefRO<StunData>>()
                    .WithAll<StunTag>())
            {
                float multiplier = moveSpeedData.ValueRO.multiplier - stunData.ValueRO.speedDebuff;
                if (multiplier < 0)
                    multiplier = 0;

                moveSpeedData.ValueRW.multiplier = multiplier;
            }
        }
    }
}