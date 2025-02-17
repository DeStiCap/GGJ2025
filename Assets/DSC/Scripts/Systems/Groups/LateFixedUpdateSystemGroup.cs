using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(FixedUpdateSystemGroup))]
    public partial class LateFixedUpdateSystemGroup : ComponentSystemGroup
    {

    }
}