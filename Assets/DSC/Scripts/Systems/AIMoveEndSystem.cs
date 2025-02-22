using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct AIMoveEndSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var query = SystemAPI.QueryBuilder()
                .WithAllRW<MoveCooldownData>()
                .WithAll<AIMoveTag, MoveTimeData>()
                .WithNone<MoveCooldownTag>()
                .Build();

            if (query.IsEmpty)
                return;

            float time = Time.time;

            var random = new Unity.Mathematics.Random((uint)DateTime.UtcNow.Ticks & 0x00000000FFFFFFFF);

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var entities = query.ToEntityArray(Allocator.Temp);
            var moveTimeDatas = query.ToComponentDataArray<MoveTimeData>(Allocator.Temp);
            var moveCooldownDatas = query.ToComponentDataArray<MoveCooldownData>(Allocator.Temp);


            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var moveTimeData = moveTimeDatas[i];
                var moveCooldownData = moveCooldownDatas[i];

                if (time < moveTimeData.endTime)
                    continue;

                ecb.RemoveComponent<AIMoveTag>(entity);
                ecb.AddComponent(entity, new MoveCooldownTag());

                moveCooldownData.endTime = time + random.NextFloat(0.25f, 1);
                ecb.SetComponent(entity, moveCooldownData);
            }


            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            entities.Dispose();
            moveTimeDatas.Dispose();
            moveCooldownDatas.Dispose();
        }
    }
}