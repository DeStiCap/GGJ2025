using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.VisualScripting;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Set Vector2 Value Random",
        story: "Set [Variable] value random by range",
        category: "Action/DSC",
        id: "1b2459d5ce08e47a6cbf0d2df2ee47fa")]
    public partial class SetVector2ValueRandomAction : Action
    {
        [SerializeReference] public BlackboardVariable<Vector2> Variable;
        [SerializeReference] public BlackboardVariable<Vector2> RandomRangeX;
        [SerializeReference] public BlackboardVariable<Vector2> RandomRangeY;

        protected override Status OnStart()
        {
            var randomX = UnityEngine.Random.Range(RandomRangeX.Value.x, RandomRangeX.Value.y);
            var randomY = UnityEngine.Random.Range(RandomRangeY.Value.x, RandomRangeY.Value.y);
            
            Variable.Value = new Vector2(randomX, randomY);

            return Status.Success;
        }
    }

}