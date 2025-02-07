using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Follow Target",
        story: "[Agent] follow [Target] at [MoveSpeed]",
        category: "Action/Physics",
        id: "c4a90a7fb82376da437ab81f1ec5125e")]
    public partial class FollowTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Rigidbody2D> Agent;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<float> MoveSpeed = new BlackboardVariable<float>(5f);
        [SerializeReference] public BlackboardVariable<float> FollowDuration = new BlackboardVariable<float>(3f);
        [SerializeReference] public BlackboardVariable<SpriteRenderer> Body;

        float m_StartTime;

        protected override Status OnStart()
        {
            if(Agent.ObjectValue == null)
            {
                LogFailure("No rigidbody2D in agent to assigned.");
                return Status.Failure;
            }

            if(Target.ObjectValue == null)
            {
                LogFailure("No target assigned.");
                return Status.Failure;
            }

            m_StartTime = Time.time;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if(Target.ObjectValue == null)
            {
                return Status.Failure;
            }

            var direction = ((Vector2)Target.Value.position - Agent.Value.position).normalized;

            Vector2 move = direction * MoveSpeed.Value * Time.fixedDeltaTime;
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

            if(Time.time >= m_StartTime + FollowDuration.Value)
            {
                return Status.Success;
            }

            return Status.Running;
        }

    }
}