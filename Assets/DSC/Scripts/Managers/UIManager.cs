using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace GGJ2025
{
    public sealed class UIManager : MonoBehaviour
    {
        #region Variable

        [SerializeField] MainCanvasController m_MainCanvasPrefab;
        [SerializeField] EventSystem m_MainEventSystemPrefab;

        static UIManager m_Instance;

        public static MainCanvasController mainCanvas
        {
            get
            {
                if (m_Instance == null)
                    return null;

                return m_Instance.m_MainCanvas;
            }
        }

        MainCanvasController m_MainCanvas;
        EventSystem m_MainEventSystem;

        #endregion

        #region Main

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitOnLoad()
        {
            SpawnIfNull();
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

            Init();

            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            if (m_MainEventSystem)
            {                
                var eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

                if (eventSystems.Length > 1)
                {
                    m_MainEventSystem.gameObject.SetActive(false);
                }
                else
                {
                    m_MainEventSystem.gameObject.SetActive(true);
                }
            }
        }

        void Init()
        {
            m_MainCanvas = Instantiate(m_MainCanvasPrefab, transform);

            m_MainEventSystem = Instantiate(m_MainEventSystemPrefab, transform);

            var eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

            if(eventSystems.Length > 1)
            {
                m_MainEventSystem.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Helper

        static void SpawnIfNull()
        {
            if (m_Instance == null)
            {
                var prefab = Resources.Load<GameObject>("Prefabs/UIManager");
                if (prefab != null)
                {
                    Instantiate(prefab);
                }
            }
        }

        #endregion
    }
}