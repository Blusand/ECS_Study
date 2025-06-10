using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson10
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(GameObjectRespawnSystemGroup))]
    public partial class GameObjectRespawnSystem : SystemBase
    {
        private int m_Index = 0;
        private float m_Timer = 0f;
        private GameObject m_Obj = null;

        protected override void OnUpdate()
        {
            foreach (var grc in SystemAPI.Query<GameObjectRespawnControllerData>())
            {
                if (m_Obj == null)
                {
                    m_Obj = GameObject.Instantiate(grc.Prefabs[m_Index % grc.Prefabs.Length]);
                }

                m_Timer += SystemAPI.Time.DeltaTime;
                if (m_Timer >= grc.Timer)
                {
                    GameObject.Destroy(m_Obj);
                    m_Obj = null;
                    ++m_Index;
                    m_Timer = 0f;
                }
            }
        }
    }
}