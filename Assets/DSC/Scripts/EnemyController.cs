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

        [Min(0.01f)]
        [SerializeField] float m_SearchDistance = 15f;

        [Min(0.01f)]
        [SerializeField] float m_GiveUpDistance = 40f;

        [SerializeField] EnemyBehaviourSO m_BehaviourTypeSO;

        
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

        Animator m_Animator;
        Rigidbody2D m_Rigidbody;
        Transform m_Target;

        Coroutine m_BehaviourCoroutine;

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
                m_Target = playerGO.transform;
            }
        }

        private void FixedUpdate()
        {
            if (m_Target == null || m_BehaviourTypeSO == null)
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

            StopCoroutine(m_BehaviourCoroutine);
            m_BehaviourCoroutine = null;
        }

        public void SetTarget(Transform target)
        {
            m_Target = target;
        }

        #endregion
    }
}