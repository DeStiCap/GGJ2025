using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "AnglerFishBehaviourType1", menuName = "DSC/Enemy Behaviour/Angler Fish Behaviour Type 1")]
    public class AnglerFishBehaviourType1 : EnemyBehaviourSO
    {
        #region Variable


        #endregion

        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            enemy.onTriggerStayEvent += OnTriggerStayEvent;
        }

        public override void UpdateBehaviour(EnemyController enemy)
        {
            
        }

        public override void DestroyBehaviour(EnemyController enemy)
        {
            enemy.onTriggerStayEvent -= OnTriggerStayEvent;
        }


        public override void OnStopCoroutine(EnemyController enemy)
        {

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

        #endregion
    }
}