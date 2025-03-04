using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(FixedUpdateSystemGroup))]
    public partial struct PlayerMoveSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var randomData = SystemAPI.GetSingletonRW<RandomData>();
            var random = randomData.ValueRO.randomArr[0];

            float deltaTime = Time.fixedDeltaTime;

            foreach(var (
                gameObjectData, moveSpeedData,
                moveSpeedAccelData, inputData) 
                in SystemAPI.Query<
                    GameObjectData, RefRW<MoveSpeedData>,
                    RefRO<MoveSpeedAccelData>, RefRO<InputData>>()
                    .WithAll<PlayerTag>())
            {
                if (gameObjectData.rigidbody == null)
                    continue;

                var accelRange = moveSpeedAccelData.ValueRO.randomRange;
                float randomModifier = random.NextFloat(accelRange.x, accelRange.y);
                float accelSpeed = randomModifier * deltaTime;

                accelSpeed *= moveSpeedData.ValueRO.multiplier;

                moveSpeedData.ValueRW.multiplier = 1f;


                var velocity = (float2)gameObjectData.rigidbody.linearVelocity;

                velocity.x += accelSpeed * inputData.ValueRO.axis.x;
                velocity.y += accelSpeed * inputData.ValueRO.axis.y;



                // Limit speed cap            
                if (moveSpeedData.ValueRO.limit.HasValue
                    && math.all(velocity != float2.zero))
                {

                    float speed = math.length(velocity);


                    var moveSpeedLimit = moveSpeedData.ValueRO.limit.Value;
                    if (speed > moveSpeedLimit)
                    {
                        var direction = math.normalize(velocity);
                        velocity = direction * moveSpeedLimit;
                    }
                }

                gameObjectData.rigidbody.linearVelocity = velocity;

            }

            randomData.ValueRW.randomArr[0] = random;
        }
    }
}