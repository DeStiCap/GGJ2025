using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2025
{
    public class EnemyGroupController : MonoBehaviour
    {
        #region Variable

        [SerializeField] Vector2 m_PatrolOffsetLimitX = new Vector2(-1,1);
        [SerializeField] Vector2 m_PatrolOffsetLimitY = new Vector2(-1,1);

        [SerializeField] bool m_RegisterGroupCount = true;

        List<EnemyController> m_EnemyList = new List<EnemyController>();

        #endregion

        #region Main

        public void OnEnemySpawn(EnemyController enemy)
        {
            if(m_EnemyList.Count <= 0 && m_RegisterGroupCount)
            {
                EnemyManager.enemyGroupCount++;
            }

            m_EnemyList.Add(enemy);

            var limitX = new Vector2(transform.position.x + m_PatrolOffsetLimitX.x, transform.position.x + m_PatrolOffsetLimitX.y);
            var limitY = new Vector2(transform.position.y + m_PatrolOffsetLimitY.x, transform.position.y + m_PatrolOffsetLimitY.y);

            enemy.SetPatrolLimit(limitX, limitY);

            enemy.RegisterGroup(this);
        }

        public void EnemyDead(EnemyController enemy)
        {            
            m_EnemyList.Remove(enemy);

            if(m_EnemyList.Count <= 0)
            {
                var mainCanvas = UIManager.mainCanvas;

                if (mainCanvas)
                {
                    //mainCanvas.ShowPopupText("KILL ALL!!", 4f);
                }

                EnemyManager.enemyGroupCount--;
            }
        }

        #endregion
    }
}