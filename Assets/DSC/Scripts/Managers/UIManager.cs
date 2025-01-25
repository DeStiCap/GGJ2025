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

        MainCanvasController m_MainCanvas;

        #endregion

        #region Main

        [RuntimeInitializeOnLoadMethod()]
        static void InitOnLoad()
        {
            if(m_Instance == null)
            {
                var prefab = Resources.Load<GameObject>("Prefabs/UIManager");
                if(prefab != null)
                {
                    Instantiate(prefab);
                }
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

            Init();
        }

        void Init()
        {
            m_MainCanvas = Instantiate(m_MainCanvasPrefab);

            var eventSystem = FindAnyObjectByType<EventSystem>();
            if(eventSystem == null)
            {
                Instantiate(m_MainEventSystemPrefab);
            }
        }

        #endregion
    }
}