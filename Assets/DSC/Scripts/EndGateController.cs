using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace GGJ2025
{
    public class EndGateController : MonoBehaviour
    {
        #region Variable

        [SerializeField] GameObject m_Gate;

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

        private void Update()
        {
            if (m_Gate)
            {
                var rot = m_Gate.transform.eulerAngles;
                rot.z -= Time.deltaTime * 30;
                m_Gate.transform.eulerAngles = rot;
            }
        }

        void OnBossDead()
        {
            if (m_Gate)
            {
                m_Gate.SetActive(true);
            }
            //GameManager.GameWin();
        }

        void OnGameWin()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("EndingCutscene");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.GameWin();
            }
        }


        #endregion
    }
}