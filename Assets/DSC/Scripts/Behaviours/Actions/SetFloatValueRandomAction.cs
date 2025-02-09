using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Set Float Value Random",
        story: "Set [Variable] value random by [RandomRange]",
        category: "Action/DSC",
        id: "eec950928afa88326260920256a54888")]
    public partial class SetFloatValueRandomAction : Action
    {
        [SerializeReference] public BlackboardVariable<float> Variable;
        [SerializeReference] public BlackboardVariable<Vector2> RandomRange;

        protected override Status OnStart()
        {
            Variable.Value = UnityEngine.Random.Range(RandomRange.Value.x, RandomRange.Value.y);

            return Status.Success;
        }

    }

}