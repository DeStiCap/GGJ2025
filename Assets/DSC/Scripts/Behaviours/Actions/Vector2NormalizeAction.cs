using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Vector2 Normalize", 
        story: "Normalize [Variable] value", 
        category: "Action/DSC", 
        id: "a990a01b00e914aec5c8a396636a879b")]
    public partial class Vector2NormalizeAction : Action
    {
        [SerializeReference] public BlackboardVariable<Vector2> Variable;

        protected override Status OnStart()
        {
            Variable.Value = Variable.Value.normalized;
            return Status.Success;
        }
    }

}