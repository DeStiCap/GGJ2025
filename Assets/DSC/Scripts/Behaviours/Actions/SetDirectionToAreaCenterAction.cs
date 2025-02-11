using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Set Direction to Area Center", 
        story: "Set [Target] direction [Direction] to area center", 
        category: "Action/DSC", 
        id: "b6ea8bf6993ce61aee7c6a5f378ffbb7")]
    public partial class SetDirectionToAreaCenterAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<Vector2> Direction;
        [SerializeReference] public BlackboardVariable<Vector2> AreaRangeX;
        [SerializeReference] public BlackboardVariable<Vector2> AreaRangeY;

        protected override Status OnStart()
        {
            if(Target.Value == null)
            {
                LogFailure("No target assigned");
                return Status.Failure;
            }

            float centerX = AreaRangeX.Value.x + (AreaRangeX.Value.y - AreaRangeX.Value.x) * 0.5f;
            float centerY = AreaRangeY.Value.x + (AreaRangeY.Value.y - AreaRangeY.Value.x) * 0.5f;

            Vector2 position = Target.Value.position;
            var center = new Vector2(centerX, centerY);


            Direction.Value = (center - position).normalized;

            return Status.Success;
        }
    }
}
