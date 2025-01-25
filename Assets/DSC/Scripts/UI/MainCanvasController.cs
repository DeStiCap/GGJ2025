using TMPro;
using UnityEngine;

namespace GGJ2025
{
    public class MainCanvasController : MonoBehaviour
    {
        #region Variable

        [SerializeField] GameObject m_GameOverUI;
        [SerializeField] TextMeshProUGUI m_PopupText;

        float? m_endPopupTextTime;

        #endregion

        #region Main

        private void OnEnable()
        {
            GameManager.onGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameManager.onGameOver -= OnGameOver;
        }

        private void Update()
        {
            if (m_endPopupTextTime.HasValue
                && Time.time >= m_endPopupTextTime.Value)
            {
                m_endPopupTextTime = null;
                m_PopupText.SetText("");
            }
        }

        void OnGameOver()
        {
            m_GameOverUI.SetActive(true);
        }

        public void ShowPopupText(string text, float showDuration)
        {
            m_PopupText.SetText(text);
            m_endPopupTextTime = Time.time + showDuration;
        }

        #endregion
    }
}