using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Set Direction to Target", 
        story: "Set [Direction] from [Agent] to [Target]", 
        category: "Action/DSC", 
        id: "268a4caa2fdad48caf38d48089a5e771")]
    public partial class SetDirectionToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Agent;
        [SerializeReference] public BlackboardVariable<Vector2> Direction;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            if(Agent.ObjectValue == null)
            {
                LogFailure("No agent assigned.");
                return Status.Failure;
            }

            if(Target.ObjectValue == null)
            {
                LogFailure("No target assigned.");
                return Status.Failure;
            }

            Direction.Value = (Target.Value.position - Agent.Value.position).normalized;
            return Status.Success;
        }
    }
}
