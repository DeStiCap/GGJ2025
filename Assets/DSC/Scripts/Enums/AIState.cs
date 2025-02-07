using Unity.Behavior;

namespace GGJ2025
{
    [BlackboardEnum]
    public enum AIState
    {
        Idle,
        Patrol,
        Alert,
        Chase,
    }
}