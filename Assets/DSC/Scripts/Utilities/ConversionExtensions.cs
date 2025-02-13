using Unity.Mathematics;
using UnityEngine;

namespace GGJ2025
{
    public static class ConversionExtensions
    {
        public static Vector3 ToVector3(this float2 value)
        {
            return new Vector3(value.x, value.y);
        }
    }
}