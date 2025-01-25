using UnityEngine;

namespace GGJ2025
{
    public class EnemySpawnerController : MonoBehaviour
    {
        #region Variable

        [SerializeField] EnemyController[] m_EnemyPrefabs;

        [Min(1)]
        [SerializeField] int m_SpawnCountPerRound = 1;
        [SerializeField] bool m_InitSpawn = true;
        [Min(0f)]
        [SerializeField] float m_RandomAreaSizeX = 1f;
        [Min(0f)]
        [SerializeField] float m_RandomAreaSizeY = 1f;

        [Header("Loop Spawn")]
        [Min(0.01f)]
        [SerializeField] float m_LoopDelayMin = 1f;
        [Min(0.01f)]
        [SerializeField] float m_LoopDelayMax = 2f;
        [SerializeField] bool m_LoopSpawn;

        float m_NextSpawnTime;

        #endregion

        #region Main

        private void Awake()
        {
            if (m_InitSpawn)
            {
                for(int i = 0; i < m_SpawnCountPerRound; i++)
                {
                    RandomSpawnEnemyInList();
                }
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
                m_NextSpawnTime = Time.time + Random.Range(m_LoopDelayMin, m_LoopDelayMax);

                for (int i = 0; i < m_SpawnCountPerRound; i++)
                {
                    RandomSpawnEnemyInList();
                }
            }
        }

        void RandomSpawnEnemyInList()
        {
            var randomID = Random.Range(0, m_EnemyPrefabs.Length);

            var prefab= m_EnemyPrefabs[randomID];
            Instantiate(prefab, GetRandomSpawnLocation(), prefab.transform.rotation);
        }

        Vector3 GetRandomSpawnLocation()
        {
            var pos = transform.position;

            pos.x += Random.Range(-m_RandomAreaSizeX, m_RandomAreaSizeX);
            pos.y += Random.Range(-m_RandomAreaSizeY, m_RandomAreaSizeY);


            return pos;
        }

        #endregion
    }
}