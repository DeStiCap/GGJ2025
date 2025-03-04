using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    [UpdateBefore(typeof(DestroyEndFrameSystem))]
    public partial struct DestroyGameObjectSystem : ISystem
    {
        #region Main

        public void OnUpdate(ref SystemState state)
        {
            foreach(var gameObjectData in SystemAPI.Query<GameObjectData>().WithAll<DestroyEndFrameTag>())
            {
                if(gameObjectData.gameObject != null)
                {
                    gameObjectData.controller.ClearEntity();
                    GameObject.Destroy(gameObjectData.gameObject);
                }
            }
        }

        #endregion
    }
}