using Unity.Entities;

namespace GGJ2025
{
    public struct MoveTimeData : IComponentData
    {
        public float startTime;
        public float endTime;
    }
}