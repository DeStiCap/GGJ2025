using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(UpdateSystemGroup))]
    public partial struct BossAttackType1System : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float time = Time.time;

            foreach(var (bossType1Data, targetData) 
                in SystemAPI.Query<RefRW<BossType1Data>, RefRO<TargetData>>()
                .WithAll<ChaseTag>())
            {
                var targetEntity = targetData.ValueRO.value;
                if (time < bossType1Data.ValueRO.nextAuraDamageTime
                    || targetEntity == Entity.Null
                    || !SystemAPI.HasComponent<HpData>(targetEntity)
                    || !SystemAPI.HasBuffer<TakeDamageBuffer>(targetEntity))
                    continue;

                var targetHpData = SystemAPI.GetComponent<HpData>(targetEntity);
                var takeDamageBuffer = SystemAPI.GetBuffer<TakeDamageBuffer>(targetEntity);

                var nextAuraDamage = bossType1Data.ValueRO.nextAuraDamage;

                var damage = targetHpData.maxHp * nextAuraDamage * 0.01f;
                takeDamageBuffer.Add(new TakeDamageBuffer { damage = damage });


                bossType1Data.ValueRW.nextAuraDamageTime = time + bossType1Data.ValueRO.auraDamageInterval;
                bossType1Data.ValueRW.nextAuraDamage = nextAuraDamage + bossType1Data.ValueRO.auraDamagePerHpMax;
            }

        }
    }
}