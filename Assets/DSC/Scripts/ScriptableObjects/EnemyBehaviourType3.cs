using System.Collections;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "EnemyBehaviourType3", menuName = "DSC/Enemy Behaviour/Type 3")]
    public class EnemyBehaviourType3 : EnemyBehaviourSO
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
            public float darkRayNextTime;
            public DarkRayController darkRay;
            public GameObject darkMask;
            public Vector3 direction;

            public RushAttackState rushAttackState;
            public float darkRushAttackAlertTime;
            public float darkRushAttackTime;
            public float endDarkRushAttackTime;
        }

        #endregion

        #region Variable

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

        WaitForSeconds m_WaitForSearchAgain;

        #endregion

        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            m_WaitForSearchAgain = new WaitForSeconds(1f);

            var behaviourData = new AnglerFishTypeData();
            behaviourData.darkMask = Instantiate(m_DarkMaskPrefab, enemy.transform);
            behaviourData.darkMask.SetActive(false);
            enemy.ChangeBehaviourData(behaviourData);

            //enemy.ChangeAIState(EnemyAIState.Chase);
        }

        public override void UpdateBehaviour(EnemyController enemy)
        {
            switch (enemy.aiState)
            {
                case AIState.Patrol:
                    if (!enemy.hasBehaviourCoroutine)
                    {
                        enemy.StartBehaviourCoroutine(PatrolBehaviourCoroutine(enemy));
                    }
                    break;

                case AIState.Chase:
                    if (!enemy.hasBehaviourCoroutine)
                    {
                        if (!enemy.behaviourData.TryGetType(out AnglerFishTypeData behaviourData))
                            break;

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

        }

        public override void OnStopCoroutine(EnemyController enemy)
        {

        }


        public IEnumerator PatrolBehaviourCoroutine(EnemyController enemy)
        {
            do
            {
                yield return m_WaitForSearchAgain;

                if (EnemyManager.TrySearchPlayerNearby(enemy.transform.position, enemy.searchDistance, out var player))
                {
                    enemy.SetTarget(player);
                    enemy.ChangeAIState(AIState.Chase);
                    enemy.StopBehaviourCoroutine();
                    break;
                }

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
                    if (VisualManager.isDark)
                    {
                        behaviourData.darkRayNextTime = Time.time + m_DarkRayDelay;

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
                                }
                                else if (Time.time >= behaviourData.darkRushAttackAlertTime)
                                {
                                    behaviourData.darkMask.SetActive(true);
                                    behaviourData.direction = (enemy.targetPosition.ToVector3() - enemy.transform.position).normalized;
                                }
                                break;
                        }


                    }
                    else if (Time.time >= behaviourData.darkRayNextTime)
                    {
                        behaviourData.darkRayNextTime = Time.time + m_DarkRayDelay;

                        behaviourData.darkRay = Instantiate(m_DarkRayPrefab, enemy.transform.position, m_DarkRayPrefab.transform.rotation);

                        var direction = (enemy.targetPosition.ToVector3() - enemy.transform.position).normalized;

                        behaviourData.darkRay.SetMoveDirection(direction);

                        enemy.StopBehaviourCoroutine();
                        break;
                    }
                }

                yield return null;

            } while (enemy.hasBehaviourCoroutine);
        }



        #endregion
    }
}