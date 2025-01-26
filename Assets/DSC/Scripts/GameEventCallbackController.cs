using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    public class GameEventCallbackController : MonoBehaviour
    {
        #region Variable

        [SerializeField] UnityEvent m_OnAllEnemyGroupDead;

        #endregion

        #region Main

        private void OnEnable()
        {
            EnemyManager.onAllEnemyGroupDead += OnAllEnemyGroupDead;
        }

        
        private void OnDisable()
        {
            EnemyManager.onAllEnemyGroupDead -= OnAllEnemyGroupDead;
        }

        private void OnAllEnemyGroupDead()
        {
            m_OnAllEnemyGroupDead?.Invoke();
        }


        #endregion
    }
}