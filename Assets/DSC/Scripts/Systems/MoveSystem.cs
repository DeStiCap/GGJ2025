using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(LateFixedUpdateSystemGroup))]
    public partial struct MoveSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach(var (gameObjectData, moveData, entity) in SystemAPI.Query<GameObjectData, RefRW<MoveData>>()
                .WithEntityAccess())
            {                
                var move = moveData.ValueRO.value;
                if (math.all(move == float2.zero))
                    continue;

                moveData.ValueRW.value = float2.zero;

                var rigidbody = gameObjectData.rigidbody;
                if (rigidbody == null)
                    continue;

                move += (float2)rigidbody.position;
                rigidbody.MovePosition(move);
                
            }
        }
    }
}