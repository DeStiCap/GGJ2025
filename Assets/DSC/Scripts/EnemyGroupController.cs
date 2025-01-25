using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2025
{
    public class EnemyGroupController : MonoBehaviour
    {
        #region Variable



        List<EnemyController> m_EnemyList = new List<EnemyController>();

        #endregion

        #region Main

        public void OnEnemySpawn(EnemyController enemy)
        {
            m_EnemyList.Add(enemy);

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
                    mainCanvas.ShowPopupText("KILL ALL!!", 4f);
                }
            }
        }

        #endregion
    }
}