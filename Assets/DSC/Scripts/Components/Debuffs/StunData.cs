using Unity.Entities;

namespace GGJ2025
{
    public struct StunData : IComponentData
    {
        public float duration;
        public float lastStunTime;
        public float speedDebuff;
    }

    public struct StunTag : IComponentData { }
}