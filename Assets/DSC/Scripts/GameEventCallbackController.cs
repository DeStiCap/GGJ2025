using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    public class GameEventCallbackController : MonoBehaviour
    {
        #region Variable

        [SerializeField] UnityEvent m_OnAllEnemyGroupDead;
        [SerializeField] UnityEvent m_OnBossDead;

        #endregion

        #region Main

        private void OnEnable()
        {
            EnemyManager.onAllEnemyGroupDead += OnAllEnemyGroupDead;

            EnemyManager.onBossDead += OnBossDead;
        }

        
        private void OnDisable()
        {
            EnemyManager.onAllEnemyGroupDead -= OnAllEnemyGroupDead;

            EnemyManager.onBossDead -= OnBossDead;
        }

        private void OnAllEnemyGroupDead()
        {
            m_OnAllEnemyGroupDead?.Invoke();            
        }


        void OnBossDead()
        {
            m_OnBossDead?.Invoke();
        }

        #endregion
    }
}