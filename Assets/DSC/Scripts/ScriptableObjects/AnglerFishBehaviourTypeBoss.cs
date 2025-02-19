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

        public class AnglerBossData : BehaviourData
        {
            public float nextAuraDamageTime;
            public float nextAuraDamage;
        }

        #endregion

        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            enemy.onTriggerStayEvent += OnTriggerStayEvent;

            if (enemy.TryGetComponent(out StatusController statusController))
            {
                statusController.AddOnDeadCallBack(OnDead);
            }

            // Test only
            Instantiate(m_AuraParticlePrefab, enemy.transform);


            // Temp
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                enemy.SetTarget(playerGO.transform);
                enemy.ChangeAIState(AIState.Chase);
            }
            

            

            enemy.ChangeBehaviourData(new AnglerBossData());

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
            enemy.onTriggerStayEvent -= OnTriggerStayEvent;
        }

        public override void OnStopCoroutine(EnemyController enemy)
        {

        }

        void OnTriggerStayEvent(EnemyController enemy, Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (col.TryGetComponent(out StatusController statusController))
                {
                    statusController.TakeDamage(3);
                }
            }
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