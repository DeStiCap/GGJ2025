using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(UpdateSystemGroup))]
    public partial struct TriggerBlindSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach(var (triggerStayBuffer, triggerBlindData, factionData) 
                in SystemAPI.Query<
                    DynamicBuffer<OnTriggerStayBuffer>,
                    RefRO<TriggerBlindData>, RefRO<FactionData>>())
            {
                if (triggerStayBuffer.Length <= 0)
                    continue;

                foreach (var triggerData in triggerStayBuffer)
                {
                    var triggerEntity = triggerData.entity;

                    if(triggerEntity != Entity.Null
                        && SystemAPI.HasComponent<FactionData>(triggerEntity))
                    {
                        var triggerFactionID = SystemAPI.GetComponent<FactionData>(triggerEntity).id;
                        if (triggerFactionID == factionData.ValueRO.id)
                            continue;

                        VisualManager.ActiveDark(triggerBlindData.ValueRO.duration);
                    }
                }
            }
        }
    }
}