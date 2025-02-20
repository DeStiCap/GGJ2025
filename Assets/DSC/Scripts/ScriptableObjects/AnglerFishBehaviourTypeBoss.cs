using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "AnglerFishBehaviourTypeBoss", menuName = "DSC/Enemy Behaviour/Angler Fish Behaviour Type Boss")]
    public class AnglerFishBehaviourTypeBoss : EnemyBehaviourSO
    {
        #region Variable

        [SerializeField] EnemySpawnerController m_GroupSpawner;
        [SerializeField] GameObject m_AuraParticlePrefab;

        #endregion

        #region Data


        #endregion

        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            if (enemy.TryGetComponent(out StatusController statusController))
            {
                statusController.AddOnDeadCallBack(OnDead);
            }

            // Test only
            Instantiate(m_AuraParticlePrefab, enemy.transform);

            if (m_GroupSpawner)
            {
                Instantiate(m_GroupSpawner, enemy.transform);
            }
        }

        public override void UpdateBehaviour(EnemyController enemy)
        {

        }

        public override void DestroyBehaviour(EnemyController enemy)
        {

        }

        public override void OnStopCoroutine(EnemyController enemy)
        {

        }

        void OnDead()
        {
            var mainCanvas = UIManager.mainCanvas;
            if (mainCanvas)
            {
                mainCanvas.ShowPopupText("GATE OPENED!", 5f);
            }
            EnemyManager.BossDead();
        }

        #endregion
    }
}