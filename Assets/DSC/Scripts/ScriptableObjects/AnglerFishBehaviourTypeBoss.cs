using System.Collections;
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
            // Test only
            Instantiate(m_AuraParticlePrefab, enemy.transform);

            enemy.ChangeAIState(EnemyAIState.Chase);

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
                case EnemyAIState.Chase:
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
            EnemyManager.BossDead();
        }

        public override void OnStopCoroutine(EnemyController enemy)
        {

        }

        IEnumerator ChaseBehaviourCoroutine(EnemyController enemy)
        {
            do
            {
                if(enemy.target == null)
                {
                    enemy.ChangeAIState(EnemyAIState.Patrol);
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
                        if(enemy.target.TryGetComponent(out StatusController statusController))
                        {

                            var maxHp = statusController.maxHp;
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
            var direction = (enemy.target.position - enemy.transform.position).normalized;
            var move = direction * enemy.moveSpeed * Time.fixedDeltaTime;

            enemy.Move(move);
        }

        #endregion
    }
}