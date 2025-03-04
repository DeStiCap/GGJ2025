using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct PositionUpdateSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach(var (gameObjectData, positionData, modeData) in SystemAPI.Query<GameObjectData, RefRW<PositionData>, RefRO<HybridUpdateModeData>>())
            {
                if (gameObjectData.gameObject == null)
                    continue;

                

                switch (modeData.ValueRO.value)
                {
                    case HybridUpdateMode.EntityToGameObject:
                        float2 entityPosition = positionData.ValueRO.value;
                        gameObjectData.gameObject.transform.position = new Vector3(entityPosition.x, entityPosition.y);
                        break;

                    case HybridUpdateMode.GameObjectToEntity:
                        Vector3 goPosition = gameObjectData.gameObject.transform.position;
                        positionData.ValueRW.value = new float2(goPosition.x, goPosition.y);
                        break;
                }
            }
        }
    }
}