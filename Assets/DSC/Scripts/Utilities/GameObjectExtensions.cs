using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public static class GameObjectExtensions
    {
        public static bool TryGetEntity(this GameObject gameObject, out Entity entity)
        {
            entity = Entity.Null;

            return gameObject != null
                && gameObject.TryGetComponent(out EntityController entityController)
                && entityController.TryGetEntity(out entity);
        }

        public static bool TryGetEntity(this GameObject gameObject, out Entity entity, out EntityManager entityManager)
        {
            entity = Entity.Null;
            entityManager = default;

            return gameObject != null
                && gameObject.TryGetComponent(out EntityController entityController)
                && entityController.TryGetEntity(out entity, out entityManager);
        }
    }
}