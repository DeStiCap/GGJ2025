using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "EnemyBehaviourType1", menuName = "DSC/Enemy Behaviour/Type 1")]
    public class EnemyBehaviourType1 : EnemyBehaviourSO
    {
        #region Variable

        [SerializeField] AnimationCurve m_PatrolMoveCurve;
        [SerializeField] float m_SearchDistance;


        #endregion

        #region Data

        public class Type1Data : BehaviourData
        {
            public float moveStartTime;
            public float moveEndTime;
            public Vector3 moveDirection;
        }

        #endregion

        public override void InitBehaviour(EnemyController enemy)
        {
            enemy.ChangeBehaviourData(new Type1Data());

            enemy.ChangeAIState(EnemyAIState.Chase);
        }

        public override void UpdateBehaviour(EnemyController enemy)
        {
            switch (enemy.aiState)
            {
                case EnemyAIState.Patrol:


                    if (!enemy.hasBehaviourCoroutine)
                    {
                        var behaviourData = (Type1Data)enemy.behaviourData;
                        behaviourData.moveStartTime = Time.time;
                        behaviourData.moveEndTime = Time.time + m_PatrolMoveCurve.keys[m_PatrolMoveCurve.length - 1].time;
                        var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

                        behaviourData.moveDirection = direction;
                        enemy.StartBehaviourCoroutine(PatrolBehaviourCoroutine(enemy));
                    }
                    break;

                case EnemyAIState.Chase:


                    if (!enemy.hasBehaviourCoroutine && enemy.target != null)
                    {
                        var behaviourData = (Type1Data)enemy.behaviourData;
                        behaviourData.moveStartTime = Time.time;
                        behaviourData.moveEndTime = Time.time + m_PatrolMoveCurve.keys[m_PatrolMoveCurve.length - 1].time;
                        var direction = (enemy.target.position - enemy.transform.position).normalized;

                        behaviourData.moveDirection = direction;
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
                
                if(enemy.behaviourData is Type1Data)
                {
                    var behaviourData = (Type1Data)enemy.behaviourData;
                    
                    var movePos = enemy.moveSpeed * behaviourData.moveDirection * m_PatrolMoveCurve.Evaluate(Time.time - behaviourData.moveStartTime) * Time.deltaTime;
                    enemy.transform.position += movePos;

                   
                    if(Time.time >= behaviourData.moveEndTime)
                    {
                        enemy.StopBehaviourCoroutine();                     
                    }
                }

                yield return null;
            }while (enemy.hasBehaviourCoroutine);
        }

        public IEnumerator ChaseBehaviourCoroutine(EnemyController enemy)
        {
            do
            {
                if(enemy.target == null)
                {
                    enemy.StopBehaviourCoroutine();
                    enemy.ChangeAIState(EnemyAIState.Patrol);
                    break;
                }

                if(enemy.behaviourData.TryGetType(out Type1Data behaviourData))
                {
                    Vector3 pos = enemy.rigidbody.position;
                    var movePos = pos + enemy.moveSpeed * behaviourData.moveDirection * m_PatrolMoveCurve.Evaluate(Time.time - behaviourData.moveStartTime) * Time.fixedDeltaTime;
                    enemy.rigidbody.MovePosition(movePos);


                    if (Time.time >= behaviourData.moveEndTime)
                    {
                        enemy.StopBehaviourCoroutine();
                    }
                }

                yield return null;
            }while(enemy.hasBehaviourCoroutine);
        }


    }
}