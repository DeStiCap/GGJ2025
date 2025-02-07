using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Move 2D By Curve",
        story: "Move [Agent] by [Curve] curve",
        category: "Action/Physics",
        description: "Move agent rigidbody by use curve to calculate move.",
        id: "f910b6fee80c07812d5b9c78c3c1d027"
        )]
    public partial class Move2DByCurveAction : Action
    {
        [SerializeReference] public BlackboardVariable<Rigidbody2D> Agent;
        [SerializeReference] public BlackboardVariable<AnimationCurveSO> Curve;
        [SerializeReference] public BlackboardVariable<float> MoveSpeed;
        [SerializeReference] public BlackboardVariable<Vector2> MoveDirection;
        [SerializeReference] public BlackboardVariable<SpriteRenderer> Body;

        float m_MoveStartTime;

        protected override Status OnStart()
        {
            if(Agent.Value == null)
            {
                LogFailure("No target rigidbody2D assigned.");
                return Status.Failure;
            }

            if(Curve.Value == null || Curve.Value.value.length <= 0)
            {
                LogFailure("No curve data for move.");
                return Status.Failure;
            }

            m_MoveStartTime = Time.time;

            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            Vector2 direction = MoveDirection.Value;
            AnimationCurve curve = Curve.Value.value;

            float endTime = m_MoveStartTime + curve.keys[curve.length - 1].time;

            Vector2 move = direction * curve.Evaluate(Time.time - m_MoveStartTime) * MoveSpeed.Value * Time.fixedDeltaTime;

            move += Agent.Value.position;
            Agent.Value.MovePosition(move);

            if(Body.ObjectValue != null)
            {
                if(direction.x > 0)
                {
                    Body.Value.flipX = true;
                }
                else if(direction.x < 0)
                {
                    Body.Value.flipX = false;
                }
            }

           
            if(Time.time >= endTime)
            {
                return Status.Success;
            }

            return Status.Running;
        }

    }

}