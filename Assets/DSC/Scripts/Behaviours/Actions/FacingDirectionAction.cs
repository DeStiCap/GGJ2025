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
        [SerializeReference] public BlackboardVariable<SpriteRenderer> Agent;
        [SerializeReference] public BlackboardVariable<Vector2> Direction;

        protected override Status OnStart()
        {
            if(Agent.ObjectValue == null)
            {
                LogFailure("No agent SpriteRenderer assigned.");
                return Status.Failure;
            }

            if(Direction.Value.x > 0)
            {
                Agent.Value.flipX = true;
            }
            else if(Direction.Value.x < 0)
            {
                Agent.Value.flipX = false;
            }
            
            return Status.Success;
        }
    }
}
