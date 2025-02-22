using Unity.Burst;
using Unity.Entities;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(UpdateSystemGroup))]
    public partial struct TriggerDamageSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach(var (triggerStayBuffer, triggerDamageData, factionData) 
                in SystemAPI.Query<
                    DynamicBuffer<OnTriggerStayBuffer>,
                    RefRO<TriggerDamageData>, RefRO<FactionData>>())
            {
                if (triggerStayBuffer.Length <= 0)
                    continue;

                foreach(var triggerData in triggerStayBuffer)
                {
                    var triggerEntity = triggerData.entity;
                    if (triggerEntity != Entity.Null
                        && SystemAPI.HasComponent<FactionData>(triggerEntity)
                        && SystemAPI.HasBuffer<TakeDamageBuffer>(triggerEntity))
                    {
                        var triggerFactionID = SystemAPI.GetComponent<FactionData>(triggerEntity).id;
                        if (triggerFactionID == factionData.ValueRO.id)
                            continue;

                        var triggerTakeDamageBuffer = SystemAPI.GetBuffer<TakeDamageBuffer>(triggerEntity);

                        triggerTakeDamageBuffer.Add(new TakeDamageBuffer { damage = triggerDamageData.ValueRO.damage });
                    }
                }
            }
        }
    }
}