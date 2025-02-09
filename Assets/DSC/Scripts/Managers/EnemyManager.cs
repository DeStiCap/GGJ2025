using UnityEngine;
using System;
using UnityEngine.SceneManagement;


namespace GGJ2025
{
    public sealed class EnemyManager : MonoBehaviour
    {
        #region Variable

        static EnemyManager m_Instance;

        Transform m_Player;

        public static int enemyGroupCount
        {
            get
            {
                if (m_Instance == null)
                    return 0;

                return m_Instance.m_EnemyGroupCount;
            }

            set
            {
                if (m_Instance == null)
                    return;

                if (value < 0)
                    value = 0;

                if(value <= 0
                    && value < m_Instance.m_EnemyGroupCount)
                {
                    m_Instance.AllEnemyGroupAreDead();
                }

                m_Instance.m_EnemyGroupCount = value;
            }
        }

        public static int aiGroupCount
        {
            get
            {
                if (m_Instance == null)
                    return 0;

                return m_Instance.m_AIGroupCount;
            }

            set
            {
                if (m_Instance == null)
                    return;

                if (value < 0)
                    value = 0;

                if (value <= 0
                    && value < m_Instance.m_AIGroupCount)
                {
                    m_Instance.AllEnemyGroupAreDead();
                }

                m_Instance.m_AIGroupCount = value;
            }
        }

        public static event Action onAllEnemyGroupDead
        {
            add
            {
                if (m_Instance == null)
                    return;

                m_Instance.m_OnAllEnemyGroupDead += value;
            }

            remove
            {
                if (m_Instance == null)
                    return;

                m_Instance.m_OnAllEnemyGroupDead -= value;
            }
        }

        public static event Action onBossDead
        {
            add
            {
                if (m_Instance == null)
                    return;

                m_Instance.m_OnBossDead += value;
            }

            remove
            {
                if (m_Instance == null)
                    return;

                m_Instance.m_OnBossDead -= value;
            }
        }


        Action m_OnBossDead;
        Action m_OnAllEnemyGroupDead;

        int m_EnemyGroupCount;
        [SerializeField] int m_AIGroupCount;

        #endregion

        #region Main

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitOnLoad()
        {
            if(m_Instance == null)
            {
                var go = new GameObject();
                go.AddComponent<EnemyManager>();
                go.name = "EnemyManager";
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

            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            m_EnemyGroupCount = 0;
        }

        public static bool TrySearchPlayerNearby(Vector3 position, float searchDistance, out Transform player)
        {
            if (m_Instance == null)
                goto Fail;

            if(m_Instance.m_Player == null && !m_Instance.TryFindPlayer())
                goto Fail;

            var distance = (m_Instance.m_Player.transform.position - position).sqrMagnitude;

            if(distance <= Mathf.Pow(searchDistance, 2))
            {
                player = m_Instance.m_Player;
                return true;
            }


            Fail:
            player = null;
            return false;
        }

        

        bool TryFindPlayer()
        {
            if (m_Player)
                return true;

            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                m_Player = playerGO.transform;
                return true;
            }

            return false;
        }

        void AllEnemyGroupAreDead()
        {

            m_OnAllEnemyGroupDead?.Invoke();

            var mainCanvas = UIManager.mainCanvas;

            if (mainCanvas)
            {
                mainCanvas.ShowPopupText("BOSS APPEARED!!", 5f);
            }
        }

        public static void BossDead()
        {
            if (m_Instance == null)
                return;

            m_Instance.m_OnBossDead?.Invoke();
        }

        #endregion
    }
}