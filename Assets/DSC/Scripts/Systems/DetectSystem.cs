using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace GGJ2025
{
    [BurstCompile]
    public partial struct DetectSystem : ISystem
    {
        #region Main

        EntityQuery m_Query;

        EntityQuery m_PlayerQuery;

        public void OnCreate(ref SystemState state)
        {
            m_Query = state.GetEntityQuery(
                ComponentType.ReadOnly<PositionData>(),
                ComponentType.ReadWrite<DetectData>());

            m_PlayerQuery = state.GetEntityQuery(
                ComponentType.ReadOnly<PlayerTag>(),
                ComponentType.ReadOnly<PositionData>());
        }

        public void OnUpdate(ref SystemState state)
        {
            if (m_Query.CalculateEntityCount() <= 0
                || m_PlayerQuery.CalculateEntityCount() <= 0)
                return;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entities = m_Query.ToEntityArray(Allocator.Temp);
            var positionDatas = m_Query.ToComponentDataArray<PositionData>(Allocator.Temp);
            var detectDatas = m_Query.ToComponentDataArray<DetectData>(Allocator.Temp);

            var playerEntities = m_PlayerQuery.ToEntityArray(Allocator.Temp);
            var playerPositionDatas = m_PlayerQuery.ToComponentDataArray<PositionData>(Allocator.Temp);


            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var position = positionDatas[i].value;
                var detectData = detectDatas[i];

                detectData.detectEntity = Entity.Null;

                float nearestDistance = float.MaxValue;

                for (int j = 0; j < playerEntities.Length; j++)
                {
                    var playerEntity = playerEntities[j];
                    var playerPosition = playerPositionDatas[j].value;

                    float distance = math.distancesq(position, playerPosition);

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;

                        if (distance <= math.mul(detectData.range, detectData.range))
                        {
                            detectData.detectEntity = playerEntity;

                        }
                    }
                }

                ecb.SetComponent(entity, detectData);
            }


            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            entities.Dispose();
            positionDatas.Dispose();
            detectDatas.Dispose();

            playerEntities.Dispose();
            playerPositionDatas.Dispose();
        }
        

        #endregion
    }
}