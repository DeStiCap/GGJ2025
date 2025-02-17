using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public class MoveCurveData : IComponentData
    {
        public AnimationCurve value;
    }
}