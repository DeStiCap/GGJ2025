using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [InternalBufferCapacity(16)]
    public struct OnTriggerStayBuffer : IBufferElementData
    {
        public Entity entity;
    }
}