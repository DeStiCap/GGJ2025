using NUnit.Framework;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

namespace GGJ2025
{
    public class EnemyGroupController : AIGroupMB
    {
        #region Variable

        [SerializeField] Vector2 m_PatrolOffsetLimitX = new Vector2(-1,1);
        [SerializeField] Vector2 m_PatrolOffsetLimitY = new Vector2(-1,1);

        [SerializeField] bool m_RegisterGroupCount = true;

        List<EnemyController> m_EnemyList = new List<EnemyController>();
        List<BehaviorGraphAgent> m_AIList = new List<BehaviorGraphAgent>();

        public override Vector2 areaRangeX
        {
            get
            {
                var result = Vector2.zero;
                Vector3 position = transform.position;

                result.x = transform.position.x + m_PatrolOffsetLimitX.x;
                result.y = transform.position.x + m_PatrolOffsetLimitX.y;

                return result;
            }
        }

        public override Vector2 areaRangeY
        {
            get
            {
                var result = Vector2.zero;
                Vector3 position = transform.position;

                result.x = transform.position.y + m_PatrolOffsetLimitY.x;
                result.y = transform.position.y + m_PatrolOffsetLimitY.y;

                return result;
            }
        }

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

        public void OnAISpawn(BehaviorGraphAgent ai)
        {
            

            if(m_AIList.Count <= 0 && m_RegisterGroupCount)
            {
                EnemyManager.aiGroupCount++;
            }

            m_AIList.Add(ai);

            ai.SetVariableValue("AIGroupMB", (AIGroupMB)this);
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