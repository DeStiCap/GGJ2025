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

        public static event Action onGameWin
        {
            add
            {
                if (m_Instance == null)
                    return;

                m_Instance.m_OnGameWin += value;
            }

            remove
            {
                if (m_Instance == null)
                    return;

                m_Instance.m_OnGameWin -= value;
            }
        }

        Action m_OnGameOver;

        Action m_OnGameWin;
        
        #endregion

        #region Main

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
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

            DontDestroyOnLoad(this);
        }

        public static void GameWin()
        {
            if (m_Instance == null)
                return;

            Time.timeScale = 0;

            m_Instance.m_OnGameWin?.Invoke();

        }

        public static void GameOver()
        {
            if (m_Instance == null)
                return;

            // Test
            Time.timeScale = 0;

            m_Instance.m_OnGameOver?.Invoke();
        }

        #endregion
    }
}