using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace GGJ2025
{
    public struct RandomData : IComponentData
    {
        public NativeArray<Random> randomArr;
    }
}