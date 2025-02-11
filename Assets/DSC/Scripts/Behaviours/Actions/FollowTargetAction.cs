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
        story: "[Agent] follow [Target] at [MoveSpeed] for [FollowDuration]",
        category: "Action/Physics",
        id: "c4a90a7fb82376da437ab81f1ec5125e")]
    public partial class FollowTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Rigidbody2D> Agent;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<float> MoveSpeed = new BlackboardVariable<float>(5f);
        [SerializeReference] public BlackboardVariable<float> FollowDuration = new BlackboardVariable<float>(3f);

        float m_StartTime;

        protected override Status OnStart()
        {
            if(Agent.Value == null)
            {
                LogFailure("No rigidbody2D in agent to assigned.");
                return Status.Failure;
            }

            if(Target.Value == null)
            {
                LogFailure("No target assigned.");
                return Status.Failure;
            }

            m_StartTime = Time.time;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if(Target.Value == null)
            {
                return Status.Failure;
            }

            var direction = ((Vector2)Target.Value.position - Agent.Value.position).normalized;

            Vector2 move = direction * MoveSpeed.Value * Time.fixedDeltaTime;
            move += Agent.Value.position;
            Agent.Value.MovePosition(move);


            Agent.Value.transform.FacingDirection(direction);


            if (Time.time >= m_StartTime + FollowDuration.Value)
            {
                return Status.Success;
            }

            return Status.Running;
        }

    }
}