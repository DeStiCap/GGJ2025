using Unity.Entities;

namespace GGJ2025
{
    public struct BossType1Data : IComponentData
    {
        public float auraDamagePerHpMax;
        public float auraDamageInterval;
        public float nextAuraDamage;
        public float nextAuraDamageTime;
    }
}