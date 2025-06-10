using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

namespace Entities.Lesson10
{
    struct RespawnControllerData : IComponentData
    {
        public float Timer;
    }

    struct PrefabBufferElement : IBufferElementData
    {
        public EntityPrefabReference Prefab;
    }

    public class RespawnControllerAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_Spawners = null;
        [SerializeField, Range(1, 5)] private float m_Timer = 3.0f;

        public class Baker : Baker<RespawnControllerAuthoring>
        {
            public override void Bake(RespawnControllerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new RespawnControllerData
                {
                    Timer = authoring.m_Timer
                });

                var buffer = AddBuffer<PrefabBufferElement>(entity);
                foreach (var spawner in authoring.m_Spawners)
                {
                    buffer.Add(new PrefabBufferElement
                    {
                        Prefab = new EntityPrefabReference(spawner)
                    });
                }
            }
        }
    }
}