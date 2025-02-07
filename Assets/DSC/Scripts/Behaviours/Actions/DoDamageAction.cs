using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "DoDamage", 
        story: "Do [Damage] damage to [Target]", 
        category: "Action/DSC", 
        id: "2a53543469baab6cab2e5e8a938cfa71")]
    public partial class DoDamageAction : Action
    {
        [SerializeReference] public BlackboardVariable<float> Damage;
        [SerializeReference] public BlackboardVariable<GameObject> Target;

        protected override Status OnStart()
        {
            if(Target.ObjectValue == null)
            {
                LogFailure("No target assigned for do damage.");
                return Status.Failure;
            }

            
            if(!Target.Value.TryGetComponent(out IDamageable damageable))
            {
                LogFailure("Can't do damage to this target");
                return Status.Failure;
            }
            
            damageable.TakeDamage(Damage.Value);
            
            return Status.Success;
        }
    }
}
