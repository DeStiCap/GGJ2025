using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Set Velocity 2D",
        story: "Set [Target] 's velocity to [Velocity]",
        category: "Action/Physics",
        id: "cbdf2cae6469f06d96863a84272f5d0c"
        )]
    public partial class SetVelocity2DAction : Action
    {

        [SerializeReference] public BlackboardVariable<Rigidbody2D> Target;
        [SerializeReference] public BlackboardVariable<Vector2> Velocity;
        protected override Status OnStart()
        {
            if (Target.Value == null)
            {
                LogFailure("No target rigidbody2D assigned.");
                return Status.Failure;
            }

            Target.Value.linearVelocity = Velocity;
            return Status.Success;
        }
    }

}