using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    public class EntityPhysicController : MonoBehaviour
    {
        #region Variable

        [SerializeField] EntityController m_EntityController;

        [SerializeField] string[] m_TriggerTags;

        [SerializeField] GameObject[] m_IgnoreGO;

        
        #endregion

        #region Main

        private void Awake()
        {
            if (m_EntityController == null)
            {
                m_EntityController = GetComponent<EntityController>();
            }
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
            if (IsIgnoreTarget(collision.gameObject)
                && !IsTargetTag(collision.gameObject))
                return;


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
            if (IsIgnoreTarget(collision.gameObject)
                && !IsTargetTag(collision.gameObject))
                return;

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
            if (IsIgnoreTarget(collision.gameObject)
                && !IsTargetTag(collision.gameObject))
                return;


            if (m_EntityController != null
                && collision.gameObject.TryGetEntity(out Entity colEntity)
                && m_EntityController.TryGetEntity(out Entity entity, out EntityManager entityManager)
                && entityManager.TryGetBuffer(entity, out DynamicBuffer<OnTriggerExitBuffer> enterBuffer))
            {
                enterBuffer.Add(new OnTriggerExitBuffer { entity = colEntity });
            }
        }

        bool IsTargetTag(GameObject target)
        {
            if (m_TriggerTags == null
                || m_TriggerTags.Length <= 0)
                return true;

            foreach(var tag in m_TriggerTags)
            {
                if (tag == null)
                    continue;

                if (target.CompareTag(tag))
                {
                    return true;
                }
            }


            return false;
        }

        bool IsIgnoreTarget(GameObject target)
        {
            if (m_IgnoreGO == null
                || m_IgnoreGO.Length <= 0)            
                return false;
            
            foreach(var go in m_IgnoreGO)
            {
                if(go == target) 
                    return true;
            }

            return false;
        }

        #endregion
    }
}