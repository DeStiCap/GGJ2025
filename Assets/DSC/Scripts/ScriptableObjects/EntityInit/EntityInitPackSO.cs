using Unity.Entities;
using UnityEngine;

namespace GGJ2025
{
    [CreateAssetMenu(fileName = "EntityInitPackSO", menuName = "DSC/Entity Init/Pack")]
    public class EntityInitPackSO : EntityInitSO
    {
        [SerializeField] EntityInitSO[] m_InitPackSO;

        public override void Init(EntityMB entityMB, Entity entity, EntityManager entityManager)
        {
            if (m_InitPackSO == null || m_InitPackSO.Length <= 0)
                return;

            foreach(var initSO in m_InitPackSO)
            {
                if (initSO == null)
                    continue;

                initSO.Init(entityMB, entity, entityManager);
            }
        }
    }
}