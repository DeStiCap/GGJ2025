using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [InternalBufferCapacity(8)]
    public struct OnTriggerEnterBuffer : IBufferElementData
    {
        public Entity entity;
    }
}