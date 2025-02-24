using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public abstract class EntityInitSO : ScriptableObject
    {
        public abstract void Init(EntityMB entityMB, Entity entity, EntityManager entityManager);
    }
}