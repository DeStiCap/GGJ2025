using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(UpdateSystemGroup))]
    public partial class PreUpdateSystemGroup : ComponentSystemGroup
    {

    }
}