using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "EnemyBehaviourType0", menuName = "DSC/Enemy Behaviour/Type 0")]
    public class EnemyBehaviourType0 : EnemyBehaviourSO
    {


        public override void InitBehaviour(EnemyController enemy)
        {

        }

        public override void UpdateBehaviour(EnemyController enemy)
        {
            if (enemy.target == null)
                return;

            var targetPos = enemy.target.position;
            var position = enemy.transform.position;
            
            var direction = (targetPos - position).normalized;

            enemy.transform.position += enemy.moveSpeed * direction * Time.deltaTime;

        }

        public override void DestroyBehaviour(EnemyController enemy)
        {

        }
    }
}