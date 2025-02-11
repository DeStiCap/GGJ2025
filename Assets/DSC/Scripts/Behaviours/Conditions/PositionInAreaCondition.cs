using System;
using Unity.Behavior;
using UnityEngine;

namespace GGJ2025
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(
        name: "Position in Area", 
        story: "If [Target] position in area range ( X : [AreaRangeX] Y : [AreaRangeY] )", 
        category: "Conditions/DSC", 
        id: "e138b35ba899d8dbc6abff95f5d34fde")]
    public partial class PositionInAreaCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<Vector2> AreaRangeX;
        [SerializeReference] public BlackboardVariable<Vector2> AreaRangeY;

        public override bool IsTrue()
        {
            if(Target.Value == null)
            {                
                return false;
            }


            var position = Target.Value.position;


            var areaX = AreaRangeX.Value;
            var areaY = AreaRangeY.Value;

            // Swap range min max if it's not correct.
            if(areaX.x > areaX.y)
            {
                float temp = areaX.x;
                areaX.x = areaX.y;
                areaX.y = temp;
            }

            if(areaY.x > areaY.y)
            {
                float temp = areaY.x;
                areaY.x = areaY.y;
                areaY.y = temp;
            }

            return ((position.x >= areaX.x && position.x <= areaX.y)
                    && (position.y >= areaY.x && position.y <= areaY.y));
        }

    }
}