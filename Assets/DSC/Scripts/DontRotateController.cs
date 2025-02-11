using UnityEngine;

namespace GGJ2025
{
    public class DontRotateController : MonoBehaviour
    {
        #region Main

        private void Update()
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        #endregion
    }
}