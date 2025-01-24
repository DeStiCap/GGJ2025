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
            var testData = new Type1Data();

            enemy.behaviourData = testData;

            enemy.aiState = EnemyAIState.Chase;
        }

        public override void UpdateBehaviour(EnemyController enemy)
        {
            switch (enemy.aiState)
            {
                case EnemyAIState.Patrol:


                    if (enemy.currentCoroutine == null)
                    {
                        var behaviourData = (Type1Data)enemy.behaviourData;
                        behaviourData.moveStartTime = Time.time;
                        behaviourData.moveEndTime = Time.time + m_PatrolMoveCurve.keys[m_PatrolMoveCurve.length - 1].time;
                        var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

                        behaviourData.moveDirection = direction;
                        enemy.currentCoroutine = enemy.StartCoroutine(PatrolMoveCoroutine(enemy));
                    }
                    break;

                case EnemyAIState.Chase:


                    if (enemy.currentCoroutine == null && enemy.target != null)
                    {
                        var behaviourData = (Type1Data)enemy.behaviourData;
                        behaviourData.moveStartTime = Time.time;
                        behaviourData.moveEndTime = Time.time + m_PatrolMoveCurve.keys[m_PatrolMoveCurve.length - 1].time;
                        var direction = (enemy.target.position - enemy.transform.position).normalized;

                        behaviourData.moveDirection = direction;
                        enemy.currentCoroutine = enemy.StartCoroutine(PatrolMoveCoroutine(enemy));
                    }
                    break;
            }
        }

        public IEnumerator PatrolMoveCoroutine(EnemyController enemy)
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
                        enemy.StopCoroutine(enemy.currentCoroutine);
                        enemy.currentCoroutine = null;                        
                    }
                }

                yield return null;
            }while (enemy.currentCoroutine != null);
        }

        public IEnumerator ChaseMoveCoroutine(EnemyController enemy)
        {
            do
            {
                if(enemy.target == null)
                {
                    enemy.StopBehaviourCoroutine();
                    enemy.aiState = EnemyAIState.Patrol;
                    break;
                }

                if(enemy.behaviourData.TryGetType(out Type1Data behaviourData))
                {
                    var movePos = enemy.moveSpeed * behaviourData.moveDirection * m_PatrolMoveCurve.Evaluate(Time.time - behaviourData.moveStartTime) * Time.deltaTime;
                    enemy.transform.position += movePos;


                    if (Time.time >= behaviourData.moveEndTime)
                    {
                        enemy.StopBehaviourCoroutine();
                    }
                }

                yield return null;
            }while(enemy.currentCoroutine != null);
        }
    }
}