using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace GGJ2025
{
    public class EndGateController : MonoBehaviour
    {
        #region Variable


        #endregion

        #region Main

        private void OnEnable()
        {
            EnemyManager.onBossDead += OnBossDead;

            GameManager.onGameWin += OnGameWin;
        }

        private void OnDisable()
        {
            EnemyManager.onBossDead -= OnBossDead;

            GameManager.onGameWin -= OnGameWin;
        }

        void OnBossDead()
        {
            GameManager.GameWin();
        }

        void OnGameWin()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("EndingCutscene");
        }



        #endregion
    }
}