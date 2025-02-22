using Unity.Entities;

namespace GGJ2025
{
    public struct TriggerDamageData : IComponentData, IEnableableComponent
    {
        public float damage;
    }
}