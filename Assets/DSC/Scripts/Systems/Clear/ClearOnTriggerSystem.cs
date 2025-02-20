using Unity.Burst;
using Unity.Entities;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct ClearOnTriggerSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach(var (triggerEnterBuffer, triggerStayBuffer, triggerExitBuffer) 
                in SystemAPI.Query<
                    DynamicBuffer<OnTriggerEnterBuffer>, DynamicBuffer<OnTriggerStayBuffer>, DynamicBuffer<OnTriggerExitBuffer>>())
            {
                if(triggerEnterBuffer.Length > 0)
                {
                    triggerEnterBuffer.Clear();
                }

                if(triggerStayBuffer.Length > 0)
                {
                    triggerStayBuffer.Clear();
                }

                if(triggerExitBuffer.Length > 0)
                {
                    triggerExitBuffer.Clear();
                }
            }
        }
    }
}