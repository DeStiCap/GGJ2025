using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "FacingToTarget",
        description: "No facing to any direction if no target assigned.",
        story: "[Agent] facing to [Target]", 
        category: "Action/DSC", 
        id: "51b6925d2f5d506a6584965afe45e559")]
    public partial class FacingToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Agent;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            if(Agent == null || Agent.Value == null)
            {
                LogFailure("No agent assigned.");
                return Status.Failure;
            }

            if(Target == null)
            {
                LogFailure("No target assigned.");
                return Status.Failure;
            }

            if(Target.Value == null)
            {
                return Status.Success;
            }

            var direction = (Target.Value.position - Agent.Value.transform.position).normalized;

            Agent.Value.FacingDirection(direction);

            return Status.Success;
        }
    }
}
