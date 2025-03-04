using Unity.Entities;
using Unity.Mathematics;

namespace GGJ2025
{
    public struct MoveSpeedAccelData : IComponentData
    {
        public float2 randomRange;
        public float value;
        public float instability;
    }
}