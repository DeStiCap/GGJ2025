using Unity.Behavior;
using UnityEngine;

namespace GGJ2025
{
    public class AIController : AICoreMB
    {
        #region Variable

        public override AIGroupMB aiGroup
        {
            get
            {
                return m_AIGroup;
            }

            protected set
            {
                m_AIGroup = value;
            }
        }

        BehaviorGraphAgent m_Agent;
        AIGroupMB m_AIGroup;

        #endregion

        #region Main

        void Awake()
        {
            m_Agent = GetComponent<BehaviorGraphAgent>();
        }

        public override void RegisterGroup(AIGroupMB group)
        {
            base.RegisterGroup(group);

            m_Agent.SetVariableValue("AIGroupMB", group);
        }

        public void OnDead()
        {
            if (m_AIGroup != null)
            {
                m_AIGroup.AIDead(this);
            }
        }

        #endregion
    }
}