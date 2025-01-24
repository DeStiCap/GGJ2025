using UnityEngine;

namespace GGJ2025
{
    public static class Extension_BehaviourData
    {
        public static bool TryGetType<T>(this BehaviourData data, out T value) where T : BehaviourData
        {
            if(data is T)
            {
                value = (T)data;
                return true;
            }

            value = null;
            return false;
        }
    }
}