using NUnit.Framework;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GGJ2025
{
    public class EnemyGroupController : MonoBehaviour
    {
        #region Variable

        [SerializeField] Vector2 m_AreaRange = new Vector2(1, 1);

        [SerializeField] bool m_RegisterGroupCount = true;

        List<EnemyController> m_EnemyList = new List<EnemyController>();

        EntityController m_EntityController;

        #endregion

        #region Main

        private void Awake()
        {
            m_EntityController = GetComponent<EntityController>();
        }

        private void Start()
        {
            if(m_EntityController != null
                && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
            {
                entityManager.AddComponentData(entity, new AreaRangeData
                {
                    value = m_AreaRange
                });
            }
        }

        public void OnEnemySpawn(EnemyController enemy)
        {
            if(m_EnemyList.Count <= 0 && m_RegisterGroupCount)
            {
                EnemyManager.enemyGroupCount++;
            }

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
                    //mainCanvas.ShowPopupText("KILL ALL!!", 4f);
                }

                EnemyManager.enemyGroupCount--;
            }
        }

        #endregion
    }
}