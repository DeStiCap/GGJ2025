using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public struct TriggerBlindData : IComponentData, IEnableableComponent
    {
        public float duration;
    }

    public struct TriggerBlindEnableTag : IComponentData { }

    public struct TriggerBlindDisableTag : IComponentData { }

    public struct EndMoveTriggerBlindDisableTag : IComponentData { }
}