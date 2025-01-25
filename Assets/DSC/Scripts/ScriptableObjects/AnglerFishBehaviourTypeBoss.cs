using System.Collections;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "AnglerFishBehaviourTypeBoss", menuName = "DSC/Enemy Behaviour/Angler Fish Behaviour Type Boss")]
    public class AnglerFishBehaviourTypeBoss : EnemyBehaviourSO
    {
        #region Main

        public override void InitBehaviour(EnemyController enemy)
        {

        }

        public override void UpdateBehaviour(EnemyController enemy)
        {

        }

        public override void DestroyBehaviour(EnemyController enemy)
        {

        }

        public override void OnStopCoroutine(EnemyController enemy)
        {

        }

        IEnumerator PatrolBehaviourCoroutine(EnemyController enemy)
        {
            yield return null;
        }



        #endregion
    }
}