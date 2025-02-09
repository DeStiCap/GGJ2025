using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "FacingDirection", 
        story: "[Agent] facing to [Direction]", 
        category: "Action/DSC", 
        id: "8bcd74443e8dd38282c282ca92c996a8")]
    public partial class FacingDirectionAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Agent;
        [SerializeReference] public BlackboardVariable<Vector2> Direction;

        protected override Status OnStart()
        {
            if(Agent.ObjectValue == null)
            {
                LogFailure("No agent assigned.");
                return Status.Failure;
            }

            Agent.Value.FacingDirection(Direction.Value);

            return Status.Success;
        }
    }
}
