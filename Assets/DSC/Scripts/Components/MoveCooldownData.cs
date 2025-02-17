using Unity.Entities;

namespace GGJ2025
{
    public struct MoveCooldownData : IComponentData
    {
        public float endTime;
    }
}