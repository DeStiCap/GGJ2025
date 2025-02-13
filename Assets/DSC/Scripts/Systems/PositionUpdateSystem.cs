using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct PositionUpdateSystem : ISystem
    {
        #region Main

        EntityQuery m_Query;

        public void OnCreate(ref SystemState state)
        {
            m_Query = state.GetEntityQuery(
                ComponentType.ReadOnly<PositionUpdateModeData>(),
                ComponentType.ReadWrite<PositionData>(),
                ComponentType.ReadWrite<GameObjectData>());
        }

        public void OnUpdate(ref SystemState state)
        {
            if (m_Query.CalculateEntityCount() <= 0)
                return;

            foreach(var (gameObjectData, positionData, modeData) in SystemAPI.Query<GameObjectData, RefRW<PositionData>, RefRO<PositionUpdateModeData>>())
            {
                if (gameObjectData.gameObject == null)
                    continue;

                

                switch (modeData.ValueRO.value)
                {
                    case PositionUpdateMode.EntityToGameObject:
                        float2 entityPosition = positionData.ValueRO.value;
                        gameObjectData.gameObject.transform.position = new Vector3(entityPosition.x, entityPosition.y);
                        break;

                    case PositionUpdateMode.GameObjectToEntity:
                        Vector3 goPosition = gameObjectData.gameObject.transform.position;
                        positionData.ValueRW.value = new float2(goPosition.x, goPosition.y);
                        break;
                }
            }
        }

        #endregion
    }
}