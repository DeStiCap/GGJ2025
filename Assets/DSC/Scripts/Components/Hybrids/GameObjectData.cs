using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public class GameObjectData : IComponentData
    {
        public GameObject gameObject;
        public EntityController controller;
    }
}