using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace GGJ2025
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Move 2D", 
        story: "Move [Agent] toward [MoveDirection] at [MoveSpeed] for [MoveDuration]", 
        category: "Action/DSC", 
        id: "a9049028c633028333ab0d5acbca129e")]
    public partial class Move2DAction : Action
    {
        [SerializeReference] public BlackboardVariable<Rigidbody2D> Agent;        
        [SerializeReference] public BlackboardVariable<float> MoveSpeed = new BlackboardVariable<float>(5f);
        [SerializeReference] public BlackboardVariable<Vector2> MoveDirection;
        [SerializeReference] public BlackboardVariable<float> MoveDuration = new BlackboardVariable<float>(1f);

        float m_MoveStartTime;

        protected override Status OnStart()
        {
            if (Agent.ObjectValue == null)
            {
                LogFailure("No agent rigidbody2D assigned.");
                return Status.Failure;
            }


            m_MoveStartTime = Time.time;

            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            Vector2 move = MoveDirection.Value * MoveSpeed.Value * Time.fixedDeltaTime;
            move += Agent.Value.position;
            Agent.Value.MovePosition(move);

            Agent.Value.transform.FacingDirection(MoveDirection.Value);

            if(Time.time > m_MoveStartTime + MoveDuration.Value)
            {
                return Status.Success;
            }

            return Status.Running;
        }
    }
}
