using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Set Area Range by Group",
        description: "If AIGroupMB not assigned. Area range will return same as before.",
        story: "Set area ( X : [AreaRangeX] Y : [AreaRangeY] ) by [AIGroupMB]", 
        category: "Action/DSC", 
        id: "463bc0a5a321ab000cb18442a49c2765")]
    public partial class SetAreaRangeByGroupAction : Action
    {
        [SerializeReference] public BlackboardVariable<AIGroupMB> AIGroupMB;
        [SerializeReference] public BlackboardVariable<Vector2> AreaRangeX;
        [SerializeReference] public BlackboardVariable<Vector2> AreaRangeY;

        protected override Status OnStart()
        {
            if(AIGroupMB == null || AIGroupMB.Value == null)
            {
                return Status.Success;
            }

            AreaRangeX.Value = AIGroupMB.Value.areaRangeX;
            AreaRangeY.Value = AIGroupMB.Value.areaRangeY;

            
            return Status.Success;
        }
    }
}
