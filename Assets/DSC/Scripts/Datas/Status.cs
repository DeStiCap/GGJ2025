using UnityEngine;

namespace GGJ2025
{
    [System.Serializable]
    public struct Status
    {
        public float hp;
        [Min(1)]
        public float maxHp;
    }
}