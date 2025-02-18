using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public partial struct ChangeAIStateByTargetSystem : ISystem
    {
        EntityQuery m_Query;

        public void OnCreate(ref SystemState state)
        {
            m_Query = state.GetEntityQuery(
                ComponentType.ReadOnly<TargetData>(),
                ComponentType.ReadOnly<GameObjectData>(),
                ComponentType.ReadWrite<AIStateData>(),
                ComponentType.Exclude<LockAIStateTag>());
        }

        public void OnUpdate(ref SystemState state)
        {
            if (m_Query.CalculateEntityCount() <= 0)
                return;

            var ecb = new EntityCommandBuffer(Allocator.Temp);


            foreach(var (gameObjectData, aiStateData, targetData, entity) 
                in SystemAPI.Query<GameObjectData, RefRW<AIStateData>, RefRO<TargetData>>()
                .WithNone<LockAIStateTag>()
                .WithEntityAccess())
            {
                switch (aiStateData.ValueRO.value)
                {
                    case AIState.Patrol:
                        if(targetData.ValueRO.value != Entity.Null
                            && gameObjectData.gameObject != null
                            && gameObjectData.gameObject.TryGetComponent(out EnemyController enemyController))
                        {
                            aiStateData.ValueRW.nextValue = AIState.Chase;
                            ecb.AddComponent(entity, new AIMoveStartTag());

                            enemyController.StopBehaviourCoroutine();
                        }
                        break;
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();


        }
    }
}