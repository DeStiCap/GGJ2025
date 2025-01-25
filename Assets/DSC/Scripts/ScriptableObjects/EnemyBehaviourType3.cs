using System.Collections;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "EnemyBehaviourType3", menuName = "DSC/Enemy Behaviour/Type 3")]
    public class EnemyBehaviourType3 : EnemyBehaviourSO
    {
        #region Data

        public class Type3Data : BehaviourData
        {
            public float startMoveTime;
            public float endMoveTime;
            public Vector3 direction;

            
        }

        #endregion

        #region Variable

        [Min(0.01f)]
        [SerializeField] float m_ChaseMoveDuration = 1f;

        //[SerializeField] AnimationCurve m_ChaseReadyMoveCurve;


        #endregion



        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            var behaviourData = new Type3Data();
            enemy.behaviourData = behaviourData;

            enemy.ChangeAIState(EnemyAIState.Chase);
        }

        public override void UpdateBehaviour(EnemyController enemy)
        {
            switch (enemy.aiState)
            {
                case EnemyAIState.Chase:
                    if(enemy.currentCoroutine == null)
                    {
                        if (!enemy.behaviourData.TryGetType(out Type3Data behaviourData))
                            return;

                        behaviourData.startMoveTime = Time.time;
                        behaviourData.endMoveTime = Time.time + m_ChaseMoveDuration;
                        behaviourData.direction = (enemy.target.position - enemy.transform.position).normalized;

                        enemy.StartCoroutine(ChaseBehaviourCoroutine(enemy));
                    }
                    break;
            }
        }

        public IEnumerator ChaseBehaviourCoroutine(EnemyController enemy)
        {
            do
            {
                if(enemy.target == null)
                {
                    enemy.StopBehaviourCoroutine();
                    break;
                }

                if(enemy.behaviourData.TryGetType(out Type3Data behaviourData))
                {
                    Vector3 pos = enemy.rigidbody.position;
                    var movePos = pos + enemy.moveSpeed * behaviourData.direction * Time.deltaTime;
                    enemy.rigidbody.MovePosition(movePos);

                    if(Time.time >= behaviourData.endMoveTime)
                    {
                        enemy.StopBehaviourCoroutine();
                        break;
                    }
                }


                yield return null;

            }while(enemy.currentCoroutine != null);
        }

        #endregion
    }
}