using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "AnimationCurveSO", menuName = "DSC/ Animation Curve SO")]
    public class AnimationCurveSO : ScriptableObject
    {
        [SerializeField] AnimationCurve m_Value;

        public AnimationCurve value { get { return m_Value; } }
    }
}