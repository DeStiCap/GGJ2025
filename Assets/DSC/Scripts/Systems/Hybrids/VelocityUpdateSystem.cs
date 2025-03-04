using Unity.Entities;

namespace GGJ2025
{
    [UpdateInGroup(typeof(LateFixedUpdateSystemGroup), OrderLast = true)]
    public partial struct VelocityUpdateSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach(var (
                gameObjectData, velocityData, 
                modeData, entity) 
                in SystemAPI.Query<
                    GameObjectData, RefRW<VelocityData>,
                    RefRO<HybridUpdateModeData>>()
                    .WithEntityAccess())
            {
                if (gameObjectData.rigidbody == null)
                    continue;

                switch (modeData.ValueRO.value)
                {
                    case HybridUpdateMode.EntityToGameObject:
                        gameObjectData.rigidbody.linearVelocity = velocityData.ValueRO.value;
                        break;

                    case HybridUpdateMode.GameObjectToEntity:
                        velocityData.ValueRW.value = gameObjectData.rigidbody.linearVelocity;
                        break;
                }
            }
        }
    }
}