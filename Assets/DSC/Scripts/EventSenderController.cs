using UnityEngine;

namespace GGJ2025
{
    public class EventSenderController : MonoBehaviour
    {

        #region Main

        public void InvokeEvent(EventKeySO eventKey)
        {
            GameManager.InvokeEvent(eventKey.name);
        }

        #endregion
    }
}