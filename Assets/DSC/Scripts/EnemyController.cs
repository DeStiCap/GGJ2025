using System.Collections;
using UnityEngine;
using System;

namespace GGJ2025
{
    public class EnemyController : Enemy
    {
        #region Variable

        [SerializeField] Animator m_Animator;
        [Min(0)]
        [SerializeField] float m_MoveSpeed = 5f;

        [Min(0.01f)]
        [SerializeField] float m_SearchDistance = 15f;

        [Min(0.01f)]
        [SerializeField] float m_GiveUpDistance = 40f;

        [SerializeField] EnemyBehaviourSO m_BehaviourTypeSO;

        [SerializeField] SpriteRenderer m_SpriteRenderer;

        [SerializeField] EnemyAIState m_AIState;

        public float moveSpeed { get { return m_MoveSpeed; } }

        public float searchDistance { get { return m_SearchDistance; } }

        public float giveUpDistance { get { return m_GiveUpDistance; } }

        public Transform target { get { return m_Target; } }

        public EnemyAIState aiState { get { return m_AIState; } }

        public bool hasBehaviourCoroutine { get { return m_BehaviourCoroutine != null; } }

        public BehaviourData behaviourData { get { return m_BehaviourData; } }

        public Animator animator { get { return m_Animator; } }
        public new Rigidbody2D rigidbody { get { return m_Rigidbody; } }

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

        public Vector2? patrolLimitX { get { return m_PatrolLimitX; } }
        public Vector2? patrolLimitY { get { return m_PatrolLimitY; } }

        Rigidbody2D m_Rigidbody;
       

        Transform m_Target;

        Coroutine m_BehaviourCoroutine;

        BehaviourData m_BehaviourData;

        EnemyGroupController m_GroupController;

        Action<EnemyController, Collider2D> m_OnTriggerEnterEvent;
        Action<EnemyController, Collider2D> m_OnTriggerStayEvent;
        Action<EnemyController, Collider2D> m_OnTriggerExitEvent;

        Vector2? m_PatrolLimitX;
        Vector2? m_PatrolLimitY;

        #endregion

        #region Main

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody2D>();
            

            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO)
            {
                m_Target = playerGO.transform;
            }
        }

        private void Start()
        {
            if (m_BehaviourTypeSO)
            {
                m_BehaviourTypeSO.InitBehaviour(this);
            }
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
            if (m_Target == null || m_BehaviourTypeSO == null)
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

        public void ChangeAIState(EnemyAIState aiState)
        {
            m_AIState = aiState;
        }

        public void ChangeBehaviourData(BehaviourData behaviourData)
        {
            m_BehaviourData = behaviourData;
        }

        public bool IsTargetOutOfRange()
        {
            if (m_Target == null)
                return true;

            var distance = (m_Target.position - transform.position).sqrMagnitude;

            return distance > Mathf.Pow(m_GiveUpDistance, 2);
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

            move += m_Rigidbody.position;

            m_Rigidbody.MovePosition(move);            
        }

        public void MoveLimit(Vector2 move)
        {
            var movePos = new Vector2(transform.position.x + move.x, transform.position.y + move.y);

            movePos.x = patrolLimitX.HasValue ? Mathf.Clamp(movePos.x, patrolLimitX.Value.x, patrolLimitX.Value.y) : movePos.x;
            movePos.y = patrolLimitY.HasValue ? Mathf.Clamp(movePos.y, patrolLimitY.Value.x, patrolLimitY.Value.y) : movePos.y;

            move = new Vector2(movePos.x - transform.position.x, movePos.y - transform.position.y);

            Move(move);
        }

        public void SetTarget(Transform target)
        {
            m_Target = target;
        }

        public void RegisterGroup(EnemyGroupController group)
        {
            m_GroupController = group;
        }

        public void SetPatrolLimit(Vector2 limitX, Vector2 limitY)
        {
            m_PatrolLimitX = limitX;
            m_PatrolLimitY = limitY;
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