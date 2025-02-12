using Unity.Entities;

namespace GGJ2025
{
    public struct HpData : IComponentData
    {
        public float hp;
        public float maxHp;
    }
}