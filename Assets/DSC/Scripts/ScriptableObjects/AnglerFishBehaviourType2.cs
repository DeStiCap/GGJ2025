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

        #endregion

        #region Variable


        #endregion


        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {
            enemy.onTriggerEnterEvent += OnTriggerEnterEvent;


        }

        public override void UpdateBehaviour(EnemyController enemy)
        {


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
                if(enemy.entityController != null
                    && enemy.entityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                    && entityManager.TryGetComponentData(entity, out AIStateData aiStateData)
                    && entityManager.TryGetComponentData(entity, out MoveTimeData moveTimeData)
                    && entityManager.TryGetComponentData(entity, out ChargeAttackData chargeAttackData))
                {
                    if(aiStateData.value == AIState.Chase
                        && Time.time >= moveTimeData.startTime + chargeAttackData.chargeDuration)
                    {
                        VisualManager.ActiveDark(2f);
                    }
                }

            }
        }

        #endregion

    }
}