using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public partial struct SetTargetByDetectSystem : ISystem
    {
        #region Main

        EntityQuery m_Query;

        public void OnCreate(ref SystemState state)
        {
            m_Query = state.GetEntityQuery(
                ComponentType.ReadOnly<DetectData>(),
                ComponentType.ReadWrite<TargetData>(),                
                ComponentType.Exclude<CantSetTargetTag>());
        }

        public void OnUpdate(ref SystemState state)
        {
            if (m_Query.CalculateEntityCount() <= 0)
                return;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entities = m_Query.ToEntityArray(Allocator.Temp);
            var detectDatas = m_Query.ToComponentDataArray<DetectData>(Allocator.Temp);
            var targetDatas = m_Query.ToComponentDataArray<TargetData>(Allocator.Temp);
            


            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var targetData = targetDatas[i];
                var detectData = detectDatas[i];

                if (targetData.value != Entity.Null)
                    continue;

                if(detectData.detectEntity != Entity.Null)
                {
                    targetData.value = detectData.detectEntity;
                    ecb.SetComponent(entity, targetData);
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            entities.Dispose();
            detectDatas.Dispose();
            targetDatas.Dispose();
            
        }

        #endregion
    }
}