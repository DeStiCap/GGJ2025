using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public class EntityController : EntityMB
    {
        #region Variable

        [SerializeField] HybridUpdateMode m_HybridUpdateMode = HybridUpdateMode.GameObjectToEntity;
        [SerializeField] EntityInitSO[] m_EntityInits;

        [Header("Hybrid")]
        [SerializeField] Animator m_Animator;
        [SerializeField] Rigidbody2D m_Rigidbody;

        #endregion

        #region Main

        public Entity entity { get { return m_Entity; } }

        Entity m_Entity;

        private void Awake()
        {
            if(m_Animator == null)
            {
                m_Animator = GetComponent<Animator>();
            }

            if(m_Rigidbody == null)
            {
                m_Rigidbody = GetComponent<Rigidbody2D>();
            }

            if(HybridManager.TryGetEntityManager(out EntityManager entityManager))
            {
                m_Entity = entityManager.CreateEntity();
                entityManager.SetName(m_Entity, gameObject.name);

                entityManager.AddComponentObject(m_Entity, new GameObjectData
                {
                    gameObject = gameObject,
                    controller = this,
                    animator = m_Animator,
                    rigidbody = m_Rigidbody,
                });

                entityManager.AddComponentData(entity, new PositionData
                {
                    value = (Vector2)transform.position
                });

                entityManager.AddComponentData(entity, new HybridUpdateModeData
                {
                    value = m_HybridUpdateMode,
                });

                if(m_EntityInits != null)
                {
                    foreach(var entityInit in m_EntityInits)
                    {
                        if (entityInit == null)
                            continue;

                        entityInit.Init(this, entity, entityManager);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if(m_Entity != Entity.Null
                && HybridManager.TryGetEntityManager(out EntityManager entityManager))
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

            return HybridManager.TryGetEntityManager(out entityManager);
        }

        public void ClearEntity()
        {
            m_Entity = Entity.Null;
        }

        #endregion
    }
}