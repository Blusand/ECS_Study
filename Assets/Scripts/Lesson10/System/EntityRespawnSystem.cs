using Unity.Burst;
using Unity.Entities;
using Unity.Scenes;

namespace Entities.Lesson10
{
    [BurstCompile]
    [UpdateInGroup(typeof(EntityRespawnSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct EntityRespawnSystem : ISystem, ISystemStartStop
    {
        private int m_Index;
        private float m_Timer;
        private Entity m_ControllerEntity;
        private Entity m_InstanceEntity;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RespawnControllerData>();
        }

        [BurstCompile]
        public void OnStartRunning(ref SystemState state)
        {
            m_Index = 0;
            m_Timer = 0f;
            m_InstanceEntity = default;
            m_ControllerEntity = default;

            var prefabs = SystemAPI.GetSingletonBuffer<PrefabBufferElement>(true);
            m_ControllerEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData<RequestEntityPrefabLoaded>(m_ControllerEntity,
                new RequestEntityPrefabLoaded
                {
                    Prefab = prefabs[m_Index % prefabs.Length].Prefab
                });
            state.EntityManager.AddComponent<RespawnCleanupComponentData>(m_ControllerEntity);
            ++m_Index;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!m_ControllerEntity.Equals(default))
            {
                if (state.EntityManager.HasComponent<PrefabLoadResult>(m_ControllerEntity))
                {
                    if (!m_InstanceEntity.Equals(default))
                    {
                        state.EntityManager.DestroyEntity(m_InstanceEntity);
                    }

                    var data = state.EntityManager.GetComponentData<PrefabLoadResult>(m_ControllerEntity);
                    m_InstanceEntity = state.EntityManager.Instantiate(data.PrefabRoot);
                    state.EntityManager.DestroyEntity(m_ControllerEntity);
                    m_Timer = 0f;
                }

                var controller = SystemAPI.GetSingleton<RespawnControllerData>();
                m_Timer += SystemAPI.Time.DeltaTime;
                if (m_Timer >= controller.Timer)
                {
                    var prefabs = SystemAPI.GetSingletonBuffer<PrefabBufferElement>(true);
                    state.EntityManager.AddComponentData<RequestEntityPrefabLoaded>(m_ControllerEntity,
                        new RequestEntityPrefabLoaded
                        {
                            Prefab = prefabs[m_Index % prefabs.Length].Prefab
                        });
                    ++m_Index;
                    m_Timer = 0f;
                }
            }
        }

        [BurstCompile]
        public void OnStopRunning(ref SystemState state)
        {
            if (!m_InstanceEntity.Equals(default))
            {
                state.EntityManager.DestroyEntity(m_InstanceEntity);
                m_InstanceEntity = default;
            }

            if (!m_ControllerEntity.Equals(default))
            {
                state.EntityManager.DestroyEntity(m_ControllerEntity);
                m_ControllerEntity = default;
            }

            m_Timer = 0f;
            m_Index = 0;
        }
    }
}