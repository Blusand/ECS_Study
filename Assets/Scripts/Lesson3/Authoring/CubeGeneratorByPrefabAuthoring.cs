using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson3
{
    struct CubeGeneratorByPrefabData : IComponentData
    {
        public Entity CubeEntityProtoType;
        public int CubeCount;
    }

    public class CubeGeneratorByPrefabAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject m_CubePrefab = null;
        [SerializeField, Range(1, 10)] private int m_CubeCount = 6;

        class Baker : Baker<CubeGeneratorByPrefabAuthoring>
        {
            public override void Bake(CubeGeneratorByPrefabAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new CubeGeneratorByPrefabData
                {
                    CubeEntityProtoType = GetEntity(authoring.m_CubePrefab, TransformUsageFlags.Dynamic),
                    CubeCount = authoring.m_CubeCount
                });
            }
        }
    }
}