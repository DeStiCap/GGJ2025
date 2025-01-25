using UnityEngine;

namespace GGJ2025
{
    public abstract class EnemyBehaviourSO : ScriptableObject
    {
        public abstract void InitBehaviour(EnemyController enemy);
        public abstract void UpdateBehaviour(EnemyController enemy);

        public abstract void DestroyBehaviour(EnemyController enemy);
    }
}