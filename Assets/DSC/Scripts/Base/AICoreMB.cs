using UnityEngine;

namespace GGJ2025
{
    public abstract class AICoreMB : MonoBehaviour
    {
        public abstract AIGroupMB aiGroup { get; protected set; }

        public virtual void RegisterGroup(AIGroupMB group)
        {
            aiGroup = group;
        }
    }
}