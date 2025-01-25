using System.Collections;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "AnglerFishBehaviourType3", menuName = "DSC/Enemy Behaviour/Angler Fish Behaviour Type 3")]
    public class AnglerFishBehaviourType3 : EnemyBehaviourSO
    {
        #region Variable

        [SerializeField] AnimationCurve m_PatrolMoveCurve;

        #endregion

        #region Data

        public class AnglerFishBehaviourData : BehaviourData
        {
            public float moveStartTime;
            public float moveEndTime;
            public Vector3 direction;
        }

        #endregion

        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            enemy.ChangeBehaviourData(new AnglerFishBehaviourData());

            enemy.ChangeAIState(EnemyAIState.Chase);
        }

        public override void UpdateBehaviour(EnemyController enemy)
        {
            switch (enemy.aiState)
            {
                case EnemyAIState.Patrol:


                    if (!enemy.hasBehaviourCoroutine)
                    {
                        var behaviourData = (AnglerFishBehaviourData)enemy.behaviourData;
                        behaviourData.moveStartTime = Time.time;
                        behaviourData.moveEndTime = Time.time + m_PatrolMoveCurve.keys[m_PatrolMoveCurve.length - 1].time;
                        var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

                        behaviourData.direction = direction;
                        enemy.StartBehaviourCoroutine(PatrolBehaviourCoroutine(enemy));
                    }
                    break;

                case EnemyAIState.Chase:


                    if (!enemy.hasBehaviourCoroutine && enemy.target != null)
                    {
                        var behaviourData = (AnglerFishBehaviourData)enemy.behaviourData;
                        behaviourData.moveStartTime = Time.time;
                        behaviourData.moveEndTime = Time.time + m_PatrolMoveCurve.keys[m_PatrolMoveCurve.length - 1].time;
                        var direction = (enemy.target.position - enemy.transform.position).normalized;

                        behaviourData.direction = direction;
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

                if (enemy.behaviourData is AnglerFishBehaviourData)
                {
                    var behaviourData = (AnglerFishBehaviourData)enemy.behaviourData;

                    var movePos = enemy.moveSpeed * behaviourData.direction * m_PatrolMoveCurve.Evaluate(Time.time - behaviourData.moveStartTime) * Time.deltaTime;
                    enemy.transform.position += movePos;


                    if (Time.time >= behaviourData.moveEndTime)
                    {
                        enemy.StopBehaviourCoroutine();
                    }
                }

                yield return null;
            } while (enemy.hasBehaviourCoroutine);
        }

        public IEnumerator ChaseBehaviourCoroutine(EnemyController enemy)
        {
            do
            {
                if (enemy.target == null
                    || enemy.IsTargetOutOfRange())
                {
                    enemy.StopBehaviourCoroutine();
                    enemy.ChangeAIState(EnemyAIState.Patrol);
                    break;
                }

                if (enemy.behaviourData.TryGetType(out AnglerFishBehaviourData behaviourData))
                {
                    Vector3 pos = enemy.rigidbody.position;
                    var movePos = pos + enemy.moveSpeed * behaviourData.direction * m_PatrolMoveCurve.Evaluate(Time.time - behaviourData.moveStartTime) * Time.fixedDeltaTime;
                    enemy.rigidbody.MovePosition(movePos);


                    if (Time.time >= behaviourData.moveEndTime)
                    {
                        enemy.StopBehaviourCoroutine();
                    }
                }

                yield return null;
            } while (enemy.hasBehaviourCoroutine);
        }



        #endregion
    }
}