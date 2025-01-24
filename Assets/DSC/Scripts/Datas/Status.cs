using UnityEngine;

namespace GGJ2025
{
    [System.Serializable]
    public struct Status
    {
        public int hp;
        [Min(1)]
        public int maxHp;
        [Min(0)]
        public float moveSpeed;
    }
}