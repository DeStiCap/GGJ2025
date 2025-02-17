using Unity.Entities;

namespace GGJ2025
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(LateSimulationSystemGroup))]
    public partial class UpdateSystemGroup : ComponentSystemGroup
    {

    }
}