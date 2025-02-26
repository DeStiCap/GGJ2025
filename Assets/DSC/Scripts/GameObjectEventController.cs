using UnityEngine;

namespace GGJ2025
{
    public class GameObjectEventController : MonoBehaviour
    {
        #region Main

        public void Destroy()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}