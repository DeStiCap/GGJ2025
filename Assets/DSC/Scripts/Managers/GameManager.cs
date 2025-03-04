using UnityEngine;
using System;
using Unity.Entities;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Events;

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

        Dictionary<string, UnityEvent> m_EventDic = new Dictionary<string, UnityEvent>();

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

            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

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

        public static void RegisterEventCallback(string eventName, UnityAction callback)
        {
            if (m_Instance == null)
                return;

            if(m_Instance.m_EventDic.TryGetValue(eventName, out UnityEvent gameEvent))
            {
                gameEvent.AddListener(callback);
            }
            else
            {
                var newEvent = new UnityEvent();
                newEvent.AddListener(callback);

                m_Instance.m_EventDic.Add(eventName, newEvent);
            }
        }

        public static void UnregisterEventCallback(string eventName, UnityAction callback)
        {
            if (m_Instance == null)
                return;

            if (m_Instance.m_EventDic.TryGetValue(eventName, out UnityEvent gameEvent))
            {
                gameEvent.RemoveListener(callback);
            }
        }

        public static void InvokeEvent(string eventName)
        {
            if (m_Instance == null 
                || m_Instance.m_EventDic == null
                || m_Instance.m_EventDic.Count <= 0)
                return;

            if(m_Instance.m_EventDic.TryGetValue(eventName, out UnityEvent gameEvent))
            {
                gameEvent?.Invoke();
            }
        }

        #endregion
    }
}