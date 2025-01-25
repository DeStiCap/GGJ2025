using UnityEngine;

namespace GGJ2025
{
    public class LifeTimeController : MonoBehaviour
    {
        #region Variable

        [Min(0.01f)]
        [SerializeField] float m_LifeTimeDuration = 1f;

        float m_endLifeTime;

        #endregion

        #region Main

        private void Awake()
        {
            m_endLifeTime = Time.time + m_LifeTimeDuration;
        }

        private void Update()
        {
            if (Time.time >= m_endLifeTime)
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}