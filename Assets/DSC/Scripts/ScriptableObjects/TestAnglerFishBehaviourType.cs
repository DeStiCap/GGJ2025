using System.Collections;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "TestAnglerFishBehaviourType", menuName = "DSC/Enemy Behaviour/Test Angler Fish Behaviour Type")]
    public class TestAnglerFishBehaviourType : EnemyBehaviourSO
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

            public float darkRayNextTime;
            public DarkRayController darkRay;
            public GameObject darkMask;
            public Vector3 direction;

            public RushAttackState rushAttackState;
            public float darkRushAttackAlertTime;
            public float darkRushAttackTime;
            public float endDarkRushAttackTime;

            public int attackPatternID;

            public float endAttackPatternTime;
        }

        #endregion

        #region Variable

        [SerializeField] AnimationCurve m_PatrolMoveCurve;
        [SerializeField] DarkRayController m_DarkRayPrefab;
        [SerializeField] GameObject m_DarkMaskPrefab;
        [Min(0.01f)]
        [SerializeField] float m_DarkRayDelay = 2f;

        [Min(0.01f)]
        [SerializeField] float m_RushAttackAlertDelay = 0.5f;

        [Min(0.01f)]
        [SerializeField] float m_RushAttackDelay = 1f;

        [Min(0.01f)]
        [SerializeField] float m_RushAttackDuration = 0.5f;
        [Min(1f)]
        [SerializeField] float m_RushAttackSpeedMultiplier = 2f;

        float m_SearchInterval = 1f;

        #endregion

        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            enemy.onTriggerEnterEvent += OnTriggerEnterEvent;
            var behaviourData = new AnglerFishTypeData();
            behaviourData.darkMask = Instantiate(m_DarkMaskPrefab, enemy.transform);
            behaviourData.darkMask.SetActive(false);
            enemy.ChangeBehaviourData(behaviourData);
        }


        public override void UpdateBehaviour(EnemyController enemy)
        {
            switch (enemy.aiState)
            {
                case AIState.Patrol:
                    if (!enemy.hasBehaviourCoroutine)
                    {
                        if (!enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                            break;

                        behaviourData.moveStartTime = Time.time;
                        behaviourData.moveEndTime = Time.time + m_PatrolMoveCurve.keys[m_PatrolMoveCurve.length - 1].time;
                        var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

                        behaviourData.direction = direction;

                        behaviourData.searchNextTime = Time.time + m_SearchInterval;

                        enemy.StartBehaviourCoroutine(PatrolBehaviourCoroutine(enemy));
                    }
                    break;

                case AIState.Chase:
                    if (!enemy.hasBehaviourCoroutine)
                    {
                        if (!enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                            break;

                        behaviourData.attackPatternID = GetRandomAttackPatternID(out float attackPatternDuration);

                        behaviourData.endAttackPatternTime = Time.time + attackPatternDuration;

                        behaviourData.darkRayNextTime = Time.time + m_DarkRayDelay;
                        behaviourData.direction = (enemy.targetPosition.ToVector3() - enemy.transform.position).normalized;
                        behaviourData.rushAttackState = RushAttackState.Ready;
                        behaviourData.darkMask.SetActive(false);

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

        }

        void OnTriggerEnterEvent(EnemyController enemy, Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                {

                    if (behaviourData.attackPatternID == 1
                        && behaviourData.rushAttackState == RushAttackState.Attack)
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
                    var movePos = (Vector3)enemy.rigidbody.position + behaviourData.direction * m_PatrolMoveCurve.Evaluate(Time.time - behaviourData.moveStartTime) * enemy.moveSpeed * Time.fixedDeltaTime;
                    enemy.rigidbody.MovePosition(movePos);

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

        public IEnumerator ChaseBehaviourCoroutine(EnemyController enemy)
        {
            do
            {
                if (!enemy.hasTarget
                    || (enemy.IsTargetOutOfRange()))
                {
                    enemy.StopBehaviourCoroutine();
                    enemy.ChangeAIState(AIState.Patrol);
                    break;
                }

                if (enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                {
                    switch (behaviourData.attackPatternID)
                    {
                        case 0:
                            AttackPattern1(enemy, behaviourData);
                            break;

                        case 1:
                            AttackPattern2(enemy, behaviourData);
                            break;
                    }

                }

                yield return null;

            } while (enemy.hasBehaviourCoroutine);
        }

        void AttackPattern1(EnemyController enemy, AnglerFishTypeData behaviourData)
        {
            var direction = (enemy.targetPosition.ToVector3() - enemy.transform.position).normalized;
            var movePos = (Vector3)enemy.rigidbody.position + direction * enemy.moveSpeed * Time.fixedDeltaTime;
            enemy.rigidbody.MovePosition(movePos);


            if (Time.time >= behaviourData.endAttackPatternTime)
            {
                enemy.StopBehaviourCoroutine();
            }
        }

        void AttackPattern2(EnemyController enemy, AnglerFishTypeData behaviourData)
        {
            switch (behaviourData.rushAttackState)
            {
                case RushAttackState.Ready:
                    behaviourData.rushAttackState = RushAttackState.Attack;

                    behaviourData.darkMask.SetActive(false);

                    behaviourData.darkRushAttackAlertTime = Time.time + m_RushAttackAlertDelay;
                    behaviourData.darkRushAttackTime = Time.time + m_RushAttackDelay;
                    behaviourData.endDarkRushAttackTime = Time.time + m_RushAttackDelay + m_RushAttackDuration;


                    break;

                case RushAttackState.Attack:


                    if (Time.time >= behaviourData.darkRushAttackTime)
                    {

                        if (Time.time < behaviourData.endDarkRushAttackTime)
                        {
                            var movePos = (Vector3)enemy.rigidbody.position + behaviourData.direction * enemy.moveSpeed * m_RushAttackSpeedMultiplier * Time.fixedDeltaTime;
                            enemy.rigidbody.MovePosition(movePos);
                        }
                        else
                        {
                            behaviourData.rushAttackState = RushAttackState.Ready;
                            enemy.StopBehaviourCoroutine();
                        }
                    }
                    else if (Time.time >= behaviourData.darkRushAttackAlertTime)
                    {
                        behaviourData.darkMask.SetActive(true);
                        behaviourData.direction = (enemy.targetPosition.ToVector3() - enemy.transform.position).normalized;
                    }
                    break;
            }
        }



        #endregion

        #region Helper

        int GetRandomAttackPatternID(out float attackPatternDuration)
        {
            var randomID = Random.Range(0, 2);

            switch (randomID)
            {
                case 0:
                    attackPatternDuration = 3f;
                    break;

                default:
                    attackPatternDuration = 0.01f;
                    break;
            }

            return randomID;
        }



        #endregion

    }
}