using System.Collections;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "EnemyBehaviourType2", menuName = "DSC/Enemy Behaviour/Type 2")]
    public class EnemyBehaviourType2 : EnemyBehaviourSO
    {
        #region Data

        public class Type2Data : BehaviourData
        {
            public float startMoveTime;
            public float endMoveTime;
            public Vector3 direction;

            public float endAttackTime;
        }

        #endregion

        #region Variable

        [Min(0.01f)]
        [SerializeField] float m_ChaseMoveDuration = 1;

        [Min(0.01f)]
        [SerializeField] float m_AttackDuration = 1;

        #endregion        

        public override void InitBehaviour(EnemyController enemy)
        {
            enemy.ChangeBehaviourData(new Type2Data());

            enemy.ChangeAIState(EnemyAIState.Chase);        
        }

        public override void UpdateBehaviour(EnemyController enemy)
        {
            switch (enemy.aiState)
            {
                case EnemyAIState.Chase:
                    if(!enemy.hasBehaviourCoroutine && enemy.target != null)
                    {
                        if (!enemy.behaviourData.TryGetType(out Type2Data behaviourData))
                            return;

                        behaviourData.startMoveTime = Time.time;
                        behaviourData.endMoveTime = Time.time + m_ChaseMoveDuration;

                        behaviourData.endAttackTime = Time.time + m_ChaseMoveDuration + m_AttackDuration;

                        behaviourData.direction = (enemy.target.position - enemy.transform.position).normalized;

                        enemy.StartBehaviourCoroutine(ChaseBehaviourCoroutine(enemy));
                    }
                    break;
            }
        }


        public override void DestroyBehaviour(EnemyController enemy)
        {

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

                if(enemy.behaviourData.TryGetType(out Type2Data behaviourData))
                {
                    if (Time.time < behaviourData.endMoveTime)
                    {
                        Vector3 pos = enemy.rigidbody.position;
                        var movePos = pos + enemy.moveSpeed * behaviourData.direction * Time.fixedDeltaTime;
                        enemy.rigidbody.MovePosition(movePos);

                    }
                    else if (Time.time < behaviourData.endAttackTime)
                    {
                        enemy.animator.SetBool("Attacking", true);
                    }
                    else
                    {
                        enemy.animator.SetBool("Attacking", false);

                        enemy.StopBehaviourCoroutine();
                    }
                }

                yield return null;

            }while(enemy.hasBehaviourCoroutine);
        }

    }
}