using Unity.Entities;

namespace GGJ2025
{
    public struct ChargeAttackData : IComponentData
    {
        public float chargeDuration;
        public float chargeSpeed;
    }
}