using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public static class EntityManagerExtensions
    {
        public static bool TryGetComponentData<T>(this EntityManager entityManager, Entity entity, out T data) where T : unmanaged, IComponentData
        {
            if (!entityManager.HasComponent<T>(entity))
            {
                data = default(T);
                return false;
            }

            data = entityManager.GetComponentData<T>(entity);
            return true;
        }

        public static bool TryGetComponentObject<T>(this EntityManager entityManager, Entity entity, out T data) where T : class, IComponentData
        {
            if (!entityManager.HasComponent<T>(entity))
            {
                data = null;
                return false;
            }

            data = entityManager.GetComponentObject<T>(entity);
            return true;
        }
    }
}