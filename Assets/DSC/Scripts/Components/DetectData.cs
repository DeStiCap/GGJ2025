using Unity.Entities;

namespace GGJ2025
{
    public struct DetectData : IComponentData
    {
        public Entity detectEntity;
        public float range;
    }
}