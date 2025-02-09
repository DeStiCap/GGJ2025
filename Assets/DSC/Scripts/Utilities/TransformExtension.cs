using UnityEngine;

namespace GGJ2025
{
    public static class TransformExtension
    {
        public static void FacingDirection(this Transform transform, Vector2 direction)
        {
            if (direction.x > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else if (direction.x < 0)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }
}