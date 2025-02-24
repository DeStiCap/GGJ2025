using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "GameEventSO", menuName = "DSC/Game Event SO")]
    public class GameEventSO : ScriptableObject
    {
        public void GameOver()
        {
            GameManager.GameOver();
        }

        public void RestartScene()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void BossDead()
        {
            var mainCanvas = UIManager.mainCanvas;
            if (mainCanvas)
            {
                mainCanvas.ShowPopupText("GATE OPENED!", 5f);
            }
            EnemyManager.BossDead();
        }
    }
}
