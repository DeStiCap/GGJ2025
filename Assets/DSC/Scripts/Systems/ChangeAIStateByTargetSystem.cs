using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public partial struct ChangeAIStateByTargetSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {

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