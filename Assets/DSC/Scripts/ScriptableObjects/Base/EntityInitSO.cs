using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public abstract class EntityInitSO : ScriptableObject
    {
        public abstract void Init(Entity entity, EntityManager entityManager);
    }
}