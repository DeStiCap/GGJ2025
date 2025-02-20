using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public class EntityPhysicController : MonoBehaviour
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
                entityManager.AddBuffer<OnTriggerEnterBuffer>(entity);
                entityManager.AddBuffer<OnTriggerStayBuffer>(entity);
                entityManager.AddBuffer<OnTriggerExitBuffer>(entity);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_EntityController != null
                && collision.gameObject.TryGetEntity(out Entity colEntity)
                && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                && entityManager.TryGetBuffer(entity, out DynamicBuffer<OnTriggerEnterBuffer> enterBuffer))
            {
                enterBuffer.Add(new OnTriggerEnterBuffer { entity = colEntity });
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (m_EntityController != null
                && collision.gameObject.TryGetEntity(out Entity colEntity)
                && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                && entityManager.TryGetBuffer(entity, out DynamicBuffer<OnTriggerStayBuffer> enterBuffer))
            {
                enterBuffer.Add(new OnTriggerStayBuffer { entity = colEntity });
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_EntityController != null
                && collision.gameObject.TryGetEntity(out Entity colEntity)
                && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                && entityManager.TryGetBuffer(entity, out DynamicBuffer<OnTriggerExitBuffer> enterBuffer))
            {
                enterBuffer.Add(new OnTriggerExitBuffer { entity = colEntity });
            }
        }

        #endregion
    }
}