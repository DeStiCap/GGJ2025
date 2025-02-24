using UnityEngine;

namespace GGJ2025
{
    public class DontRotateController : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}