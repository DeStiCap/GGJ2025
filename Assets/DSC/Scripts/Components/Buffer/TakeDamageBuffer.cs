using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [InternalBufferCapacity(4)]
    public struct TakeDamageBuffer : IBufferElementData
    {
        public float damage;
    }
}