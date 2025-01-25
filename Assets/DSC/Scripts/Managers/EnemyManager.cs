using UnityEngine;

namespace GGJ2025
{
    public sealed class EnemyManager : MonoBehaviour
    {
        #region Variable

        static EnemyManager m_Instance;

        Transform m_Player;

        #endregion

        #region Main

        private void Awake()
        {
            if(m_Instance == null)
            {
                m_Instance = this;
            }
            else if(m_Instance != this)
            {
                Destroy(this);
                return;
            }
        }

        private void Start()
        {
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                m_Player = playerGO.transform;
            }
        }

        public static bool TrySearchPlayerNearby(Vector3 position, float searchDistance, out Transform player)
        {
            if (m_Instance == null || m_Instance.m_Player == null)
                goto Fail;

            var distance = (m_Instance.m_Player.transform.position - position).sqrMagnitude;

            if(distance <= Mathf.Pow(searchDistance, 2))
            {
                player = m_Instance.m_Player;
                return true;
            }


            Fail:
            player = null;
            return false;
        }

        #endregion
    }
}