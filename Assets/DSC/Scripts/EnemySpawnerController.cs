using UnityEngine;

namespace GGJ2025
{
    public class EnemySpawnerController : MonoBehaviour
    {
        #region Variable

        [SerializeField] EnemyController[] m_EnemyPrefabs;
        [Min(0.01f)]
        [SerializeField] float m_SpawnDelayMin = 1f;
        [Min(0.01f)]
        [SerializeField] float m_SpawnDelayMax = 2f;

        float m_NextSpawnTime;

        #endregion

        #region Main

        private void Update()
        {
            if(Time.time >= m_NextSpawnTime)
            {
                m_NextSpawnTime = Time.time + Random.Range(m_SpawnDelayMin, m_SpawnDelayMax);

                var randomID = Random.Range(0, m_EnemyPrefabs.Length);

                Instantiate(m_EnemyPrefabs[randomID], transform.position, Quaternion.identity);
            }
        }

        #endregion
    }
}