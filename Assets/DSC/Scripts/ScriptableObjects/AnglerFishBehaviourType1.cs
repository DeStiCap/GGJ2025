using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "AnglerFishBehaviourType1", menuName = "DSC/Enemy Behaviour/Angler Fish Behaviour Type 1")]
    public class AnglerFishBehaviourType1 : EnemyBehaviourSO
    {

        #region Data

        public class AnglerFishTypeData : BehaviourData
        {
            public float nextMoveTime;
        }

        #endregion

        #region Variable

        [SerializeField] AnimationCurve m_PatrolMoveCurve;

        [Min(0)]
        [SerializeField] Vector2 m_NextMoveDelay = new Vector2(1f,2f);

        #endregion

        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            enemy.onTriggerStayEvent += OnTriggerStayEvent;

            enemy.ChangeBehaviourData(new AnglerFishTypeData());


            if(enemy.entityController != null
                && enemy.entityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
            {
                entityManager.AddComponentData(entity, new EnemyPatrolType1Tag());
                entityManager.AddComponentData(entity, new EnemyChaseType1Tag());
                entityManager.AddComponentData(entity, new AIMoveStartTag());
                entityManager.AddComponentData(entity, new MoveTimeData());
                entityManager.AddComponentData(entity, new MoveCooldownData());
                entityManager.AddComponentObject(entity, new MoveCurveData
                {
                    value = m_PatrolMoveCurve
                });
            }
        }

        public override void UpdateBehaviour(EnemyController enemy)
        {
            if (!enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                return;

            if (Time.time < behaviourData.nextMoveTime)
                return;

            switch (enemy.aiState)
            {
                case AIState.Patrol:
                    if (!enemy.hasBehaviourCoroutine)
                    {
                        if(enemy.entityController != null
                            && enemy.entityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                        {
                            entityManager.AddComponentData(entity, new AIMoveStartTag());
                        }
                        
                        enemy.StartBehaviourCoroutine(PatrolBehaviourCoroutine(enemy));
                    }
                    break;

                case AIState.Chase:
                    if (!enemy.hasBehaviourCoroutine)
                    {
                        if (enemy.entityController != null
                            && enemy.entityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                        {
                            entityManager.AddComponentData(entity, new AIMoveStartTag());
                        }

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
            if (!enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                return;

           
        }

        void OnTriggerStayEvent(EnemyController enemy, Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (col.TryGetComponent(out StatusController statusController))
                {
                    statusController.TakeDamage(1);
                }
            }
        }


        public IEnumerator PatrolBehaviourCoroutine(EnemyController enemy)
        {
            do
            {
                
                yield return null;

            } while (enemy.hasBehaviourCoroutine);
        }

        IEnumerator ChaseBehaviourCoroutine(EnemyController enemy)
        {
            do
            {
                if(!enemy.hasTarget
                    || enemy.IsTargetOutOfRange())
                {
                    enemy.SetTarget(null);
                    enemy.ChangeAIState(AIState.Patrol);
                    enemy.StopBehaviourCoroutine();
                    break;
                }

                if (enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                {
                    AttackPattern(enemy, behaviourData);
                }

                yield return null;

            } while (enemy.hasBehaviourCoroutine);
        }

        void AttackPattern(EnemyController enemy, AnglerFishTypeData behaviourData)
        {

            //if(enemy.entityController != null
            //    && enemy.entityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
            //    && entityManager.TryGetComponentData(entity, out MoveTimeData moveTimeData)
            //    && Time.time >= moveTimeData.endTime)
            //{
            //    enemy.StopBehaviourCoroutine();
            //}
        }


        #endregion
    }
}