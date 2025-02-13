using System.Collections;
using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;

namespace GGJ2025
{
    public class EnemyController : Enemy
    {
        #region Variable

        [SerializeField] Animator m_Animator;

        [Min(0)]
        [SerializeField] float m_InitMoveSpeed = 5f;

        [Min(0.01f)]
        [SerializeField] float m_InitDetectRange = 15f;

        [Min(0.01f)]
        [SerializeField] float m_InitGiveUpRange = 30f;

        [SerializeField] EnemyBehaviourSO m_BehaviourTypeSO;

        [SerializeField] SpriteRenderer m_SpriteRenderer;

        [SerializeField] AIState m_InitAIState;

        public float moveSpeed
        {
            get
            {
                if (m_EntityController == null
                    || !m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                    return default;

                if(entityManager.TryGetComponentData(entity, out MoveSpeedData moveSpeedData))
                {
                    return moveSpeedData.value;
                }

                return default;
            }
        }

        public float searchDistance
        {
            get
            {
                if (m_EntityController == null
                    || !m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                    return default;

                if(entityManager.TryGetComponentData(entity, out DetectRangeData detectRangeData))
                {
                    return detectRangeData.value;
                }

                return default;
            }
        }

        public bool hasTarget
        {
            get
            {
                if(m_EntityController != null
                    && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                    && entityManager.TryGetComponentData(entity, out TargetData targetData))
                {
                    return targetData.value != Entity.Null;
                }

                return false;
            }
        }

        public float2 targetPosition
        {
            get
            {
                if(m_EntityController != null
                    && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                    && entityManager.TryGetComponentData(entity,out TargetData targetData)
                    && targetData.value != Entity.Null
                    && entityManager.TryGetComponentData(targetData.value, out PositionData positionData))
                {
                    return positionData.value;
                }

                return default;
            }
        }

        public AIState aiState
        {
            get
            {
                if (m_EntityController == null
                    || !m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                    return default;

                if(entityManager.TryGetComponentData(entity,out AIStateData aiStateData))
                {
                    return aiStateData.value;
                }

                return default;
            }
        }

        public bool hasBehaviourCoroutine { get { return m_BehaviourCoroutine != null; } }

        public BehaviourData behaviourData { get { return m_BehaviourData; } }

        public Animator animator
        {
            get
            {
                if (m_EntityController == null
                    || !m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                    return null;

                if(entityManager.TryGetComponentObject(entity, out GameObjectData gameObjectData))
                {
                    return gameObjectData.animator;
                }

                return null;
            }
        }

        public new Rigidbody2D rigidbody
        {
            get
            {
                if (m_EntityController == null
                    || !m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                    return null;

                if(entityManager.TryGetComponentObject(entity, out GameObjectData gameObjectData))
                {
                    return gameObjectData.rigidbody;
                }

                return null;
            }
        }

        public event Action<EnemyController, Collider2D> onTriggerEnterEvent
        {
            add
            {
                m_OnTriggerEnterEvent += value;
            }

            remove
            {
                m_OnTriggerEnterEvent -= value;
            }
        }

        public event Action<EnemyController, Collider2D> onTriggerStayEvent
        {
            add
            {
                m_OnTriggerStayEvent += value;
            }

            remove
            {
                m_OnTriggerStayEvent -= value;
            }
        }
        
        public event Action<EnemyController, Collider2D> onTriggerExitEvent
        {
            add
            {
                m_OnTriggerExitEvent += value;
            }

            remove
            {
                m_OnTriggerExitEvent -= value;
            }
        }

        public EntityController entityController { get { return m_EntityController; } }

        Rigidbody2D m_Rigidbody;

        Coroutine m_BehaviourCoroutine;

        BehaviourData m_BehaviourData;

        EnemyGroupController m_GroupController;

        Action<EnemyController, Collider2D> m_OnTriggerEnterEvent;
        Action<EnemyController, Collider2D> m_OnTriggerStayEvent;
        Action<EnemyController, Collider2D> m_OnTriggerExitEvent;

        EntityController m_EntityController;

        #endregion

        #region Main

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            
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

                entityManager.AddComponentData(entity, new DetectRangeData
                {
                    value = m_InitDetectRange,
                });

                entityManager.AddComponentData(entity, new GiveUpRangeData
                {
                    value = m_InitGiveUpRange,
                });

                entityManager.AddComponentData(entity, new AIStateData
                {
                    value = m_InitAIState,
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

                if(entityManager.TryGetComponentObject(entity, out GameObjectData gameObjectData))
                {
                    gameObjectData.animator = m_Animator;
                    gameObjectData.rigidbody = m_Rigidbody;
                }
            }

            if (m_BehaviourTypeSO)
            {
                m_BehaviourTypeSO.InitBehaviour(this);
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

        private void OnDestroy()
        {
            if (m_BehaviourTypeSO)
            {
                m_BehaviourTypeSO.DestroyBehaviour(this);
            }
        }

        private void FixedUpdate()
        {
            if (m_BehaviourTypeSO == null)
                return;

            m_BehaviourTypeSO.UpdateBehaviour(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            m_OnTriggerEnterEvent?.Invoke(this, collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            m_OnTriggerStayEvent?.Invoke(this, collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            m_OnTriggerExitEvent?.Invoke(this, collision);
        }

        void OnBossDead()
        {
            Destroy(gameObject);
        }

        public void ChangeAIState(AIState aiState)
        {
            if (m_EntityController == null
                || !m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
                return;

            if(entityManager.TryGetComponentData(entity, out AIStateData aiStateData))
            {
                aiStateData.value = aiState;

                entityManager.SetComponentData(entity, aiStateData);
            }
        }

        public void ChangeBehaviourData(BehaviourData behaviourData)
        {
            m_BehaviourData = behaviourData;
        }

        public bool IsTargetOutOfRange()
        {
            if (!hasTarget)
                return true;

            var distance = (targetPosition.ToVector3() - transform.position).sqrMagnitude;

            return distance > Mathf.Pow(m_InitGiveUpRange, 2);
        }

        public void StartBehaviourCoroutine(IEnumerator coroutine)
        {
            m_BehaviourCoroutine = StartCoroutine(coroutine);
        }

        public void StopBehaviourCoroutine()
        {
            if (m_BehaviourCoroutine == null)
                return;

            m_BehaviourTypeSO.OnStopCoroutine(this);
            StopCoroutine(m_BehaviourCoroutine);
            m_BehaviourCoroutine = null;
        }

        public void Move(Vector2 move)
        {
            if (move.x > 0)
            {
                m_SpriteRenderer.flipX = true;
            }
            else if (move.x < 0)
            {
                m_SpriteRenderer.flipX = false;
            }

            if(m_EntityController != null
                && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
            {
                if(entityManager.TryGetComponentData(entity, out MoveData moveData))
                {
                    moveData.value = move;
                    entityManager.SetComponentData(entity, moveData);
                    entityManager.AddComponentData(entity, new MoveTag());
                }
            }  
        }

        public void SetTarget(Transform target)
        {
            if(m_EntityController != null
                && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                && entityManager.TryGetComponentData(entity, out TargetData targetData))
            {
                if(target != null)
                {
                    if(target.TryGetComponent(out EntityController entityController))
                    {
                        targetData.value = entityController.entity;                        
                    }
                }
                else
                {
                    targetData.value = Entity.Null;
                }

                entityManager.SetComponentData(entity, targetData);
            }
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