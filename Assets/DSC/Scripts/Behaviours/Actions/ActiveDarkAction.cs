using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Active Dark", 
        story: "Active dark for [DarkDuration]", 
        category: "Action/DSC", 
        id: "81c11fccd935459ebe2ea718a5f1b578")]
    public partial class ActiveDarkAction : Action
    {
        [SerializeReference] public BlackboardVariable<float> DarkDuration;

        protected override Status OnStart()
        {
            VisualManager.ActiveDark(DarkDuration.Value);
            return Status.Success;
        }
    }
}
