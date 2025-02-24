using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(LateFixedUpdateSystemGroup))]
    [UpdateBefore(typeof(MoveSystem))]
    public partial struct FacingDirectionByMoveSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {

            foreach(var (gameObjectData, moveData) 
                in SystemAPI.Query<GameObjectData, RefRO<MoveData>>())
            {
                var move = moveData.ValueRO.value;
                if (math.all(move == float2.zero)
                    || gameObjectData.gameObject == null)
                    continue;

                if(move.x > 0)
                {
                    gameObjectData.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    //if (gameObjectData.gameObject.TryGetComponent(out EnemyController enemyController))
                    //{
                    //    enemyController.FlipCharacter(true);
                    //}
                }
                else if(move.x < 0)
                {
                    gameObjectData.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //if (gameObjectData.gameObject.TryGetComponent(out EnemyController enemyController))
                    //{
                    //    enemyController.FlipCharacter(false);
                    //}
                }
            }
        }

    }
}