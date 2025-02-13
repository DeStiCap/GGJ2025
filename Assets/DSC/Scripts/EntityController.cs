using Unity.Entities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace GGJ2025
{
    public class EntityController : MonoBehaviour
    {
        #region Variable

        [SerializeField] PositionUpdateMode m_PositionUpdateMode = PositionUpdateMode.GameObjectToEntity;

        #endregion

        #region Main

        public Entity entity { get { return m_Entity; } }

        Entity m_Entity;

        private void Awake()
        {
            if(GameManager.TryGetEntityManager(out EntityManager entityManager))
            {
                m_Entity = entityManager.CreateEntity();
                entityManager.SetName(m_Entity, gameObject.name);

                entityManager.AddComponentObject(m_Entity, new GameObjectData
                {
                    gameObject = gameObject,
                    controller = this,
                });

                entityManager.AddComponentData(entity, new PositionData
                {
                    value = (Vector2)transform.position
                });

                entityManager.AddComponentData(entity, new PositionUpdateModeData
                {
                    value = m_PositionUpdateMode,
                });
            }
        }

        private void OnDestroy()
        {
            if(m_Entity != Entity.Null
                && GameManager.TryGetEntityManager(out EntityManager entityManager))
            {
                entityManager.DestroyEntity(m_Entity);
                ClearEntity();
            }
        }

        public bool TryGetEntity(out Entity entity)
        {
            entity = m_Entity;
            return entity != Entity.Null;
        }

        public bool TryGetEntity(out Entity entity, out EntityManager entityManager)
        {
            if(!TryGetEntity(out entity))
            {
                entityManager = default;
                return false;
            }

            return GameManager.TryGetEntityManager(out entityManager);
        }

        public void ClearEntity()
        {
            m_Entity = Entity.Null;
        }

        #endregion
    }
}