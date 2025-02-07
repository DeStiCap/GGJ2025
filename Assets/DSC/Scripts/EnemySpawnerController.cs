using Unity.Behavior;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    public class EnemySpawnerController : MonoBehaviour
    {
        #region Variable

        [SerializeField] EnemyController[] m_EnemyPrefabs;
        [SerializeField] BehaviorGraphAgent[] m_EnemyAgents;

        [Min(1)]
        [SerializeField] int m_SpawnCountPerRound = 1;
        [SerializeField] bool m_InitSpawn = true;

        [Min(0)]
        [SerializeField] Vector2 m_RandomAreaSize;

        [Header("Loop Spawn")]
        [Min(0f)]
        [SerializeField] Vector2 m_LoopDelay;
        [SerializeField] bool m_LoopSpawn;

        [Header("Events")]
        [SerializeField] UnityEvent<EnemyController> m_OnSpawnEnemy;
        [SerializeField] UnityEvent<BehaviorGraphAgent> m_OnSpwnEnemyNew;

        float m_NextSpawnTime;

        #endregion

        #region Main

        private void Start()
        {
            if (m_InitSpawn)
            {
                m_NextSpawnTime = Time.time + Random.Range(m_LoopDelay.x, m_LoopDelay.y);
                SpawnEnemies();
            }
        }

        private void Update()
        {
            if (m_LoopSpawn)
            {
                LoopingSpawn();
            }            
        }

        void LoopingSpawn()
        {
            if (Time.time >= m_NextSpawnTime)
            {
                var loopDelay = Random.Range(m_LoopDelay.x, m_LoopDelay.y);

                m_NextSpawnTime = Time.time + loopDelay;

                SpawnEnemies();
            }
        }

        void RandomSpawnEnemyInList()
        {
            if (m_EnemyPrefabs.Length <= 0)
                return;

            var randomID = Random.Range(0, m_EnemyPrefabs.Length);

            var prefab= m_EnemyPrefabs[randomID];

            var enemy = Instantiate(prefab, GetRandomSpawnLocation(), prefab.transform.rotation);

            m_OnSpawnEnemy?.Invoke(enemy);
        }

        void NewRandomSpawnEnemyInList()
        {
            if (m_EnemyAgents.Length <= 0)
                return;

            var randomID = Random.Range(0, m_EnemyAgents.Length);

            var prefab = m_EnemyAgents[randomID];

            var enemy = Instantiate(prefab, GetRandomSpawnLocation(), prefab.transform.rotation);

            m_OnSpwnEnemyNew?.Invoke(enemy);
        }

        Vector3 GetRandomSpawnLocation()
        {
            var pos = transform.position;

            pos.x += Random.Range(-m_RandomAreaSize.x, m_RandomAreaSize.x);
            pos.y += Random.Range(-m_RandomAreaSize.y, m_RandomAreaSize.y);


            return pos;
        }

        public void SpawnEnemies()
        {
            for (int i = 0; i < m_SpawnCountPerRound; i++)
            {
                RandomSpawnEnemyInList();

                NewRandomSpawnEnemyInList();
            }
        }

        #endregion
    }
}