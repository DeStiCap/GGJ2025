using Unity.Entities;

namespace GGJ2025
{
    [InternalBufferCapacity(8)]
    public struct OnTriggerExitBuffer : IBufferElementData
    {
        public Entity entity;
    }
}