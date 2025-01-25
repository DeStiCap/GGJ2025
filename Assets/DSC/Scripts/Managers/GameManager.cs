using UnityEngine;
using System;

namespace GGJ2025
{
    public sealed class GameManager : MonoBehaviour
    {
        #region Variable

        static GameManager m_Instance;

        public static event Action onGameOver
        {
            add
            {
                if (m_Instance == null)
                    return;

                m_Instance.m_OnGameOver += value;
            }

            remove
            {
                if (m_Instance == null)
                    return;

                m_Instance.m_OnGameOver -= value;
            }
        }

        Action m_OnGameOver;

        #endregion

        #region Main

        [RuntimeInitializeOnLoadMethod()]
        static void InitOnLoad()
        {
            if(m_Instance == null)
            {
                var go = new GameObject();
                go.AddComponent<GameManager>();
                go.name = "GameManager";
            }
        }

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

        public static void GameOver()
        {
            if (m_Instance == null)
                return;

            Debug.Log("GameOver");
            m_Instance.m_OnGameOver?.Invoke();
        }

        #endregion
    }
}