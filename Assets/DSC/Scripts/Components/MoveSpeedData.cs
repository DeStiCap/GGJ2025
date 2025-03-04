using Unity.Entities;

namespace GGJ2025
{
    public struct MoveSpeedData : IComponentData
    {
        public float value;
        public float multiplier;
        public float? limit;
    }
}