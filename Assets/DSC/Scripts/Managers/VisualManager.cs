using UnityEngine;
using System;
using System.Collections;

namespace GGJ2025
{
    public sealed class VisualManager : MonoBehaviour
    {
        #region Variable

        [SerializeField] GameObject m_DarkPrefab;

        static VisualManager m_Instance;

        public static event Action<bool> onDarkChangeActive
        {
            add
            {
                if (m_Instance == null)
                    return;

                m_Instance.m_OnDarkChangeActive += value;
            }

            remove
            {
                if (m_Instance == null)
                    return;

                m_Instance.m_OnDarkChangeActive -= value;
            }
        }

        public static bool isDark
        {
            get
            {
                if(m_Instance == null)
                    return false;

                return m_Instance.m_Dark.activeSelf;
            }
        }

        Action<bool> m_OnDarkChangeActive;
        GameObject m_Dark;

        float m_endDarkTime;


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

            m_Dark = Instantiate(m_DarkPrefab);
            SetDarkActive(false);
        }

        private void Start()
        {
            StartCoroutine(InitCoroutine());

            StartCoroutine(DarkCoroutine());
        }

        void SetDarkActive(bool active)
        {
            m_Dark.SetActive(active);
            m_OnDarkChangeActive?.Invoke(active);
        }

        public static void ActiveDark(float darkDuration)
        {
            if (m_Instance == null)
                return;

            m_Instance.m_endDarkTime = Time.time + darkDuration;
            m_Instance.SetDarkActive(true);
        }
        IEnumerator InitCoroutine()
        {
            yield return null;
            //SetDarkActive(true);
        }

        IEnumerator DarkCoroutine()
        {
            do
            {
                if (m_Dark.activeSelf)
                {
                    if (Time.time >= m_endDarkTime)
                    {
                        SetDarkActive(false);
                    }
                }

                yield return null;

            }while (true);
        }

        #endregion
    }
}