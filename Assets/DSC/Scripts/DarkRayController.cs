using UnityEngine;

namespace GGJ2025
{
    public class DarkRayController : MonoBehaviour
    {
        #region Variable

        [Min(0.01f)]
        [SerializeField] float m_MoveSpeed = 5f;

        [Min(0.01f)]
        [SerializeField] float m_DarkDuration = 2f;

        Rigidbody2D m_Rigidbody;

        Vector3 m_MoveDirection;

        #endregion

        #region Main

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var movePos = (Vector3)m_Rigidbody.position + m_MoveSpeed * m_MoveDirection * Time.fixedDeltaTime;
            m_Rigidbody.MovePosition(movePos);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                VisualManager.ActiveDark(m_DarkDuration);
                Destroy(gameObject);
            }
        }

        public void SetMoveDirection(Vector3 direction)
        {
            m_MoveDirection = direction;
        }

        #endregion
    }
}