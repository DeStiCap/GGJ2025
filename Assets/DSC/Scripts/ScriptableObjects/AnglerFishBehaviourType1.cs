using System.Collections;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "AnglerFishBehaviourType1", menuName = "DSC/Enemy Behaviour/Angler Fish Behaviour Type 1")]
    public class AnglerFishBehaviourType1 : EnemyBehaviourSO
    {

        #region Data

        public class AnglerFishTypeData : BehaviourData
        {
            public float moveStartTime;
            public float moveEndTime;
            public float searchNextTime;

            public Vector3 direction;
            public float endAttackPatternTime;

            public float nextMoveTime;
        }

        #endregion

        #region Variable

        [SerializeField] AnimationCurve m_PatrolMoveCurve;

        [Min(0)]
        [SerializeField] Vector2 m_NextMoveDelay = new Vector2(1f,2f);

        float m_SearchInterval = 0.2f;

        #endregion

        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            enemy.onTriggerStayEvent += OnTriggerStayEvent;

            enemy.ChangeBehaviourData(new AnglerFishTypeData());

            enemy.ChangeAIState(EnemyAIState.Chase);
        }

        public override void UpdateBehaviour(EnemyController enemy)
        {
            if (!enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                return;

            if (Time.time < behaviourData.nextMoveTime)
                return;

            switch (enemy.aiState)
            {
                case EnemyAIState.Patrol:
                    if (!enemy.hasBehaviourCoroutine)
                    {
                        

                        behaviourData.moveStartTime = Time.time;
                        behaviourData.moveEndTime = Time.time + m_PatrolMoveCurve.keys[m_PatrolMoveCurve.length - 1].time;
                        var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

                        behaviourData.direction = direction;

                        behaviourData.searchNextTime = Time.time + m_SearchInterval;

                        enemy.StartBehaviourCoroutine(PatrolBehaviourCoroutine(enemy));
                    }
                    break;

                case EnemyAIState.Chase:
                    if (!enemy.hasBehaviourCoroutine)
                    {
                        behaviourData.endAttackPatternTime = Time.time + 3f;

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

            behaviourData.nextMoveTime = Time.time + Random.Range(m_NextMoveDelay.x, m_NextMoveDelay.y);
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
                if (enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                {
                    var move = behaviourData.direction * m_PatrolMoveCurve.Evaluate(Time.time - behaviourData.moveStartTime) * enemy.moveSpeed * Time.fixedDeltaTime;

                    enemy.Move(move);
                                   

                    if (Time.time >= behaviourData.searchNextTime)
                    {
                        behaviourData.searchNextTime = Time.time + m_SearchInterval;

                        if (EnemyManager.TrySearchPlayerNearby(enemy.transform.position, enemy.searchDistance, out var player))
                        {
                            enemy.SetTarget(player);
                            enemy.ChangeAIState(EnemyAIState.Chase);
                            enemy.StopBehaviourCoroutine();
                            break;
                        }
                    }

                    if (Time.time >= behaviourData.moveEndTime)
                    {
                        enemy.StopBehaviourCoroutine();
                        break;
                    }
                }

                yield return null;

            } while (enemy.hasBehaviourCoroutine);
        }

        IEnumerator ChaseBehaviourCoroutine(EnemyController enemy)
        {
            do
            {
                if(enemy.target == null
                    || enemy.IsTargetOutOfRange())
                {
                    enemy.ChangeAIState(EnemyAIState.Patrol);
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
            var direction = (enemy.target.position - enemy.transform.position).normalized;
            var move = direction * enemy.moveSpeed * Time.fixedDeltaTime;

            enemy.Move(move);


            if (Time.time >= behaviourData.endAttackPatternTime)
            {
                enemy.StopBehaviourCoroutine();
            }
        }


        #endregion
    }
}