using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public sealed class HybridManager : MonoBehaviour
    {
        #region Variable

        static HybridManager m_Instance;

        Entity m_RandomEntity;

        #endregion

        #region Main

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitOnLoad()
        {
            if (m_Instance == null)
            {
                var go = new GameObject();
                go.AddComponent<HybridManager>();
                go.name = "HybridManager";
            }
        }

        private void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
            }
            else if(m_Instance != this)
            {
                Destroy(this);
                return;
            }

            Init();
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            if (m_Instance != this)
                return;

            DisposeEntity();
        }

        void Init()
        {
            if (!TryGetEntityManager(out EntityManager entityManager))
                return;

            m_RandomEntity = entityManager.CreateEntity();
            entityManager.SetName(m_RandomEntity, "RandomData");

            var randomArray = new NativeArray<Unity.Mathematics.Random>(1, Allocator.Persistent);
            randomArray[0] = new Unity.Mathematics.Random((uint)DateTime.UtcNow.Ticks & 0x00000000FFFFFFFF);
            entityManager.AddComponentData(m_RandomEntity, new RandomData
            {
                randomArr = randomArray,
            });
        }

        void DisposeEntity()
        {
            if (!TryGetEntityManager(out EntityManager entityManager))
                return;

            if(m_RandomEntity != Entity.Null
                && entityManager.TryGetComponentData(m_RandomEntity, out RandomData randomData))
            {
                randomData.randomArr.Dispose();
                entityManager.DestroyEntity(m_RandomEntity);
            }
        }

        public static bool TryGetEntityManager(out EntityManager entityManager)
        {
            if (World.DefaultGameObjectInjectionWorld == null)
            {
                entityManager = default;
                return false;
            }

            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            return true;
        }

        #endregion
    }
}