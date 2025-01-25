using UnityEngine;

namespace GGJ2025
{
    public class MainCanvasController : MonoBehaviour
    {
        #region Variable

        [SerializeField] GameObject m_GameOverUI;

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

        void OnGameOver()
        {
            m_GameOverUI.SetActive(true);
        }

        #endregion
    }
}