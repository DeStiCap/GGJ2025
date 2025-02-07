using System;
using Unity.Behavior;
using UnityEngine;

namespace GGJ2025
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(
        name: "Variable is Null", 
        story: "[Variable] is null", 
        category: "Conditions/DSC", 
        id: "7bc7d8d021cfe5c178cdb73cb80a50a8")]
    public partial class VariableIsNullCondition : Condition
    {
        [SerializeReference] public BlackboardVariable Variable;


        public override bool IsTrue()
        {
            if (Variable.Type.IsValueType)
            {
                return false;
            }

            return Variable.ObjectValue is null || Variable.ObjectValue.Equals(null);
        }


    }
}