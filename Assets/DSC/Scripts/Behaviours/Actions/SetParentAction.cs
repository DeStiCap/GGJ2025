using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Set Parent", 
        story: "Set [Transform] parent to [Target]", 
        category: "Action/DSC", 
        id: "2d15d1de6412d1494ea7ccc9d957cd90")]
    public partial class SetParentAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Transform;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            if(Transform == null || Transform.Value == null)
            {
                LogFailure("No transform assigned.");
                return Status.Failure;
            }

            if(Target == null || Target.Value == null)
            {
                LogFailure("No target assigned.");
                return Status.Failure;
            }

            Transform.Value.SetParent(Target.Value);
            return Status.Success;
        }
    }
}
