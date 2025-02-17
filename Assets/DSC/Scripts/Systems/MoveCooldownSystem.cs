using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public partial struct MoveCooldownSystem : ISystem
    {
        #region Main

        public void OnUpdate(ref SystemState state)
        {
            var query = SystemAPI.QueryBuilder()
                .WithAll<MoveCooldownData, MoveCooldownTag>()
                .Build();

            if (query.IsEmpty)
                return;

            float time = Time.time;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entities = query.ToEntityArray(Allocator.Temp);
            var moveCooldownDatas = query.ToComponentDataArray<MoveCooldownData>(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var moveCooldownData = moveCooldownDatas[i];

                if (time < moveCooldownData.endTime)
                    continue;

                ecb.RemoveComponent<MoveCooldownTag>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            entities.Dispose();
            moveCooldownDatas.Dispose();
        }

        #endregion
    }
}