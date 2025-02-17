using Unity.Mathematics;

namespace GGJ2025
{
    public static class MathExtensions
    {
        public static float2 Normalize(this float2 value)
        {
            float length = math.length(value);
            return length > 0 ? value / length : float2.zero;
        }
    }
}