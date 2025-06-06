using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson9
{
    struct SpawnerGeneratorData : IComponentData
    {
        public Entity SpawnerProtoType;
        public int HalfCountX;
        public int HalfCountZ;
    }

    public class SpawnerGeneratorAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject m_SpawnerPrefab = null;
        [SerializeField, Range(10, 100)] private int m_HalfCountX = 40;
        [SerializeField, Range(10, 100)] private int m_HalfCountZ = 40;

        public class Baker : Baker<SpawnerGeneratorAuthoring>
        {
            public override void Bake(SpawnerGeneratorAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new SpawnerGeneratorData
                {
                    SpawnerProtoType = GetEntity(authoring.m_SpawnerPrefab, TransformUsageFlags.Dynamic),
                    HalfCountX = authoring.m_HalfCountX,
                    HalfCountZ = authoring.m_HalfCountZ
                });
            }
        }
    }
}