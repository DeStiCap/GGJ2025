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
        story: "[Agent] facing to [Target]", 
        category: "Action/DSC", 
        id: "51b6925d2f5d506a6584965afe45e559")]
    public partial class FacingToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<SpriteRenderer> Agent;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            if(Agent.ObjectValue == null)
            {
                LogFailure("No agent SpriteRendere assigned.");
                return Status.Failure;
            }

            if(Target.ObjectValue == null)
            {
                LogFailure("No target assigned.");
                return Status.Failure;
            }

            var direction = (Target.Value.position - Agent.Value.transform.position).normalized;

            if(direction.x > 0)
            {
                Agent.Value.flipX = true;
            }
            else if(direction.x < 0)
            {
                Agent.Value.flipX = false;
            }

            return Status.Success;
        }
    }
}
