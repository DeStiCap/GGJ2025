using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace GGJ2025
{
    public class EnemyController : Enemy
    {
        #region Variable

        [Min(0)]
        [SerializeField] float m_MoveSpeed = 5f;


        [SerializeField] EnemyBehaviourSO m_BehaviourTypeSO;

        
        [SerializeField] EnemyAIState m_AIState;

        public float moveSpeed { get { return m_MoveSpeed; } }

        public Transform target { get { return m_Player; } }

        public EnemyAIState aiState { get { return m_AIState; } }

        public Coroutine currentCoroutine {  get { return m_CurrentCoroutine; } set { m_CurrentCoroutine = value; } }

        public BehaviourData behaviourData { get { return m_BehaviourData; } set { m_BehaviourData = value; } }

        public Animator animator { get { return m_Animator; } }
        public new Rigidbody2D rigidbody { get { return m_Rigidbody; } }

        Animator m_Animator;
        Rigidbody2D m_Rigidbody;
        Transform m_Player;

        Coroutine m_CurrentCoroutine;

        BehaviourData m_BehaviourData;

        #endregion

        #region Main

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody2D>();

            if (m_BehaviourTypeSO)
            {
                m_BehaviourTypeSO.InitBehaviour(this);
            }

            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO)
            {
                m_Player = playerGO.transform;
            }
        }

        private void FixedUpdate()
        {
            if (m_Player == null || m_BehaviourTypeSO == null)
                return;

            m_BehaviourTypeSO.UpdateBehaviour(this);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if(collision.TryGetComponent(out StatusController statusController))
                {
                    statusController.TakeDamage(1);
                }
            }
        }

        public void ChangeAIState(EnemyAIState aiState)
        {
            m_AIState = aiState;
        }

        public void StopBehaviourCoroutine()
        {
            if (m_CurrentCoroutine == null)
                return;

            StopCoroutine(m_CurrentCoroutine);
            m_CurrentCoroutine = null;
        }

        #endregion
    }
}