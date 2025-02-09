using UnityEngine;

namespace GGJ2025
{
    public abstract class AIGroupMB : MonoBehaviour
    {
        public abstract Vector2 areaRangeX { get; }
        

        public abstract Vector2 areaRangeY { get; }

        public abstract void AIDead(AICoreMB ai);
    }
}