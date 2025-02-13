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
        [SerializeField] float m_AuraDamageInterval = 10f;
        [SerializeField] float m_AuraDamageHpPer = 5f;


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
            if (!enemy.behaviourData.TryGetType(out AnglerBossData behaviourData))
                return;

            switch (enemy.aiState)
            {
                case AIState.Chase:
                    if (!enemy.hasBehaviourCoroutine)
                    {
                        behaviourData.nextAuraDamageTime = Time.time + m_AuraDamageInterval;
                        behaviourData.nextAuraDamage = m_AuraDamageHpPer;

                        enemy.StartBehaviourCoroutine(ChaseBehaviourCoroutine(enemy));
                    }


                    break;
            }
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


        IEnumerator ChaseBehaviourCoroutine(EnemyController enemy)
        {
            do
            {
                if(!enemy.hasTarget)
                {
                    enemy.ChangeAIState(AIState.Patrol);
                    enemy.StopBehaviourCoroutine();
                    break;
                }

                if (enemy.IsTargetOutOfRange())
                    continue;


                if (enemy.behaviourData.TryGetType(out AnglerBossData behaviourData))
                {
                    MovePattern(enemy, behaviourData);

                    if (Time.time >= behaviourData.nextAuraDamageTime)
                    {
                        if(enemy.entityController != null
                            && enemy.entityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                            && entityManager.TryGetComponentData(entity, out TargetData targetData)
                            && targetData.value != Entity.Null
                            && entityManager.TryGetComponentData(targetData.value, out HpData hpData)
                            && entityManager.TryGetComponentObject(targetData.value, out GameObjectData gameObjectData)
                            && gameObjectData.gameObject.TryGetComponent(out StatusController statusController))
                        {
                            var maxHp = hpData.maxHp;
                            var damage = maxHp * behaviourData.nextAuraDamage / 100;
                            statusController.TakeDamage(damage);

                            behaviourData.nextAuraDamageTime = Time.time + m_AuraDamageInterval;
                            behaviourData.nextAuraDamage = behaviourData.nextAuraDamage + m_AuraDamageHpPer;
                        }
   
                    }
                }
                    

                yield return null;

            } while (enemy.hasBehaviourCoroutine);
        }

        void MovePattern(EnemyController enemy, AnglerBossData behaviourData)
        {
            var direction = (enemy.targetPosition.ToVector3() - enemy.transform.position).normalized;
            var move = direction * enemy.moveSpeed * Time.fixedDeltaTime;

            enemy.Move(move);
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