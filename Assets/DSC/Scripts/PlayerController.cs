using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public class PlayerController : MonoBehaviour
    {
        #region Variable

        EntityController m_EntityController;

        #endregion

        #region Main

        private void Awake()
        {
            m_EntityController = GetComponent<EntityController>();
        }

        private void Start()
        {
            if (m_EntityController != null
                && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager))
            {
                entityManager.AddComponentData(entity, new PlayerTag());
            }
        }

        #endregion
    }
}
