using UnityEngine;
using Unity.Entities;

namespace GGJ2025
{
    public class EnemyController : Enemy
    {
        #region Variable

        [Min(0)]
        [SerializeField] float m_InitMoveSpeed = 5f;

        [Min(0.01f)]
        [SerializeField] float m_InitDetectRange = 15f;

        [Min(0.01f)]
        [SerializeField] float m_InitGiveUpRange = 30f;

        [SerializeField] AIState m_InitAIState;


        EnemyGroupController m_GroupController;

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
                entityManager.AddComponentData(entity, new MoveData());

                entityManager.AddComponentData(entity, new MoveSpeedData
                {
                    value = m_InitMoveSpeed,
                });

                entityManager.AddComponentData(entity, new MoveDirectionData());

                entityManager.AddComponentData(entity, new DetectData
                {
                    range = m_InitDetectRange,
                });

                entityManager.AddComponentData(entity, new GiveUpRangeData
                {
                    value = m_InitGiveUpRange,
                });

                entityManager.AddComponentData(entity, new AIStateData
                {
                    nextValue = m_InitAIState,
                });

                var groupEntity = Entity.Null;
                if (m_GroupController
                    && m_GroupController.TryGetComponent(out EntityController entityController))
                {
                    groupEntity = entityController.entity;
                }

                entityManager.AddComponentData(entity, new AIGroupData
                {
                    groupEntity = groupEntity,
                });

                entityManager.AddComponentData(entity, new TargetData());
            }
        }

        private void OnEnable()
        {
            EnemyManager.onBossDead += OnBossDead;
        }

        private void OnDisable()
        {
            EnemyManager.onBossDead -= OnBossDead;
        }

        void OnBossDead()
        {
            Destroy(gameObject);
        }

        public void RegisterGroup(EnemyGroupController group)
        {
            m_GroupController = group;
        }

        public void OnDead()
        {
            if (m_GroupController)
            {
                m_GroupController.EnemyDead(this);
            }
        }

        #endregion
    }
}