using UnityEngine;

namespace GGJ2025
{
    public class VisualController : MonoBehaviour
    {
        #region Variable

        [SerializeField] GameObject m_DarkMaskPrefab;

        [SerializeField] GameObject m_PlayerCanvas;

        GameObject m_DarkMask;

        #endregion

        #region Main

        private void Awake()
        {
            m_DarkMask = Instantiate(m_DarkMaskPrefab, transform);
            m_DarkMask.SetActive(false);
        }

        private void Start()
        {
            VisualManager.onDarkChangeActive += OnDarkChangeActive;

            GameManager.onGameWin += OnGameWin;
        }

        private void OnDestroy()
        {
            VisualManager.onDarkChangeActive -= OnDarkChangeActive;

            GameManager.onGameWin -= OnGameWin;
        }

        private void OnDarkChangeActive(bool active)
        {
            m_DarkMask.SetActive(active);
        }

        void OnGameWin()
        {
            if (m_PlayerCanvas)
            {
                m_PlayerCanvas.SetActive(false);
            }
        }

        #endregion
    }
}