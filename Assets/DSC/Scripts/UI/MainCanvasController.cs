using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ2025
{
    public class MainCanvasController : MonoBehaviour
    {
        #region Variable

        [SerializeField] GameObject m_GameWinUI;
        [SerializeField] GameObject m_GameOverUI;
        [SerializeField] TextMeshProUGUI m_PopupText;

        float? m_endPopupTextTime;

        #endregion

        #region Main

        private void OnEnable()
        {
            GameManager.onGameWin += OnGameWin;
            GameManager.onGameOver += OnGameOver;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        
        private void OnDisable()
        {
            GameManager.onGameWin -= OnGameWin;
            GameManager.onGameOver -= OnGameOver;

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            if (m_GameOverUI)
            {
                m_GameOverUI.SetActive(false);
            }
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

        void OnGameWin()
        {
            m_GameWinUI.SetActive(true);
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