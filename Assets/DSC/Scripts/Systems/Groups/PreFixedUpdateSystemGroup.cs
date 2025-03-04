using Unity.Entities;

namespace GGJ2025
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(FixedUpdateSystemGroup))]
    public partial class PreFixedUpdateSystemGroup : ComponentSystemGroup
    {

    }
}