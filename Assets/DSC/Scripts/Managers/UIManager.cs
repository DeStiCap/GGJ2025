using UnityEngine;
using UnityEngine.EventSystems;

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

        [RuntimeInitializeOnLoadMethod()]
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

            DontDestroyOnLoad(this);
        }

        void Init()
        {
            m_MainCanvas = Instantiate(m_MainCanvasPrefab, transform);

            m_MainEventSystem = Instantiate(m_MainEventSystemPrefab, transform);
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