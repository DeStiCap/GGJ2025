using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "AnglerFishBehaviourType2", menuName = "DSC/Enemy Behaviour/Angler Fish Behaviour Type 2")]
    public class AnglerFishBehaviourType2 : EnemyBehaviourSO
    {
        #region Enum

        public enum RushAttackState
        {
            Ready,
            Attack
        }

        #endregion


        #region Data
        public class AnglerFishTypeData : BehaviourData
        {
            public float moveStartTime;
            public float moveEndTime;
            public float searchNextTime;

            public Vector3 direction;

            public RushAttackState rushAttackState;
            public float rushAttackTime;
            public float endRushAttackTime;

            public float nextMoveTime;
        }

        #endregion

        #region Variable

        [SerializeField] AnimationCurve m_PatrolMoveCurve;
        [Min(0)]
        [SerializeField] Vector2 m_NextMoveDelay = new Vector2(0.25f, 1f);

        [Min(0.01f)]
        [SerializeField] float m_RushAttackDelay = 1f;

        [Min(0.01f)]
        [SerializeField] float m_RushAttackDuration = 0.5f;
        [Min(1f)]
        [SerializeField] float m_RushAttackSpeedMultiplier = 2f;

        float m_SearchInterval = 0.2f;

        #endregion


        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            enemy.onTriggerEnterEvent += OnTriggerEnterEvent;

            enemy.ChangeBehaviourData(new AnglerFishTypeData());

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
                        

                        behaviourData.moveStartTime = Time.time;
                        behaviourData.moveEndTime = Time.time + m_PatrolMoveCurve.keys[m_PatrolMoveCurve.length - 1].time;
                        var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

                        if (enemy.entityController != null
                            && enemy.entityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                            && entityManager.TryGetComponentData(entity, out AIGroupData groupData))
                        {
                            var groupEntity = groupData.groupEntity;
                            if (groupEntity != Entity.Null
                                && entityManager.TryGetComponentData(groupEntity, out PositionData positionData)
                                && entityManager.TryGetComponentData(groupEntity, out AreaRangeData areaRangeData))
                            {
                                Vector3 position = enemy.transform.position;
                                Vector2 limit = positionData.value;
                                Vector2 limitX = new Vector2(limit.x - areaRangeData.value.x, limit.x + areaRangeData.value.x);
                                Vector2 limitY = new Vector2(limit.y - areaRangeData.value.y, limit.y + areaRangeData.value.y);

                                if (position.x < limitX.x
                                    || position.x > limitX.y
                                    || position.y < limitY.x
                                    || position.y > limitY.y)
                                {
                                    direction = (limit - (Vector2)position).normalized;
                                }

                            }
                        }

                        behaviourData.direction = direction;

                        behaviourData.searchNextTime = Time.time + m_SearchInterval;

                        enemy.StartBehaviourCoroutine(PatrolBehaviourCoroutine(enemy));
                    }
                    break;

                case AIState.Chase:
                    if (!enemy.hasBehaviourCoroutine)
                    {
                        behaviourData.direction = (enemy.targetPosition.ToVector3() - enemy.transform.position).normalized;
                        behaviourData.rushAttackState = RushAttackState.Ready;

                        enemy.StartBehaviourCoroutine(ChaseBehaviourCoroutine(enemy));
                    }
                    break;
            }
        }

        public override void DestroyBehaviour(EnemyController enemy)
        {
            enemy.onTriggerEnterEvent -= OnTriggerEnterEvent;
        }

        public override void OnStopCoroutine(EnemyController enemy)
        {
            if (!enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                return;

            behaviourData.nextMoveTime = Time.time + Random.Range(m_NextMoveDelay.x, m_NextMoveDelay.y);
        }


        void OnTriggerEnterEvent(EnemyController enemy, Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                {

                    if (behaviourData.rushAttackState == RushAttackState.Attack)
                    {
                        VisualManager.ActiveDark(2f);
                    }
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
                            enemy.ChangeAIState(AIState.Chase);
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
                if (!enemy.hasTarget
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
            switch (behaviourData.rushAttackState)
            {
                case RushAttackState.Ready:
                    behaviourData.rushAttackState = RushAttackState.Attack;

                    behaviourData.rushAttackTime = Time.time + m_RushAttackDelay;
                    behaviourData.endRushAttackTime = Time.time + m_RushAttackDelay + m_RushAttackDuration;


                    break;

                case RushAttackState.Attack:


                    if (Time.time >= behaviourData.rushAttackTime)
                    {

                        if (Time.time < behaviourData.endRushAttackTime)
                        {
                            var move = behaviourData.direction * enemy.moveSpeed * m_RushAttackSpeedMultiplier * Time.fixedDeltaTime;
                            enemy.Move(move);
                        }
                        else
                        {
                            behaviourData.rushAttackState = RushAttackState.Ready;
                            enemy.StopBehaviourCoroutine();
                        }
                    }
                    else
                    {

                        behaviourData.direction = (enemy.targetPosition.ToVector3() - enemy.transform.position).normalized;
                    }
                    break;
            }
        }




        #endregion

    }
}