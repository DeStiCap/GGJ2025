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
        story: "Set area ( X : [AreaRangeX] Y : [AreaRangeY] ) by [Group]", 
        category: "Action/DSC", 
        id: "463bc0a5a321ab000cb18442a49c2765")]
    public partial class SetAreaRangeByGroupAction : Action
    {
        [SerializeReference] public BlackboardVariable<AIGroupMB> Group;
        [SerializeReference] public BlackboardVariable<Vector2> AreaRangeX;
        [SerializeReference] public BlackboardVariable<Vector2> AreaRangeY;

        protected override Status OnStart()
        {
            if(Group.ObjectValue == null)
            {
                LogFailure("No group assigned.");
                return Status.Failure;
            }

            AreaRangeX.Value = Group.Value.areaRangeX;
            AreaRangeY.Value = Group.Value.areaRangeY;
            
            return Status.Success;
        }
    }
}
