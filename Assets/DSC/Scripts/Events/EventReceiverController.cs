using UnityEngine;
using UnityEngine.Events;

namespace GGJ2025
{
    public class EventReceiverController : MonoBehaviour
    {
        #region Data

        [System.Serializable]
        class EventDat
        {
            public EventKeySO eventKey;
            public UnityEvent eventCallback;

            UnityAction action;

            public void Action()
            {
                eventCallback?.Invoke();
            }

            public UnityAction GetAction()
            {
                if (action == null)
                    action = Action;

                return action;
            }
        }

        #endregion

        #region Variable

        [SerializeField] EventDat[] m_Events;

        #endregion


        #region Main

        private void OnEnable()
        {
            if (m_Events == null
                || m_Events.Length <= 0)
                return;

            foreach (var e in m_Events)
            {
                GameManager.RegisterEventCallback(e.eventKey.name, e.GetAction());
            }
        }

        private void OnDisable()
        {
            if (m_Events == null
                || m_Events.Length <= 0)
                return;

            foreach (var e in m_Events)
            {
                GameManager.UnregisterEventCallback(e.eventKey.name, e.GetAction());
            }
        }

        #endregion
    }
}