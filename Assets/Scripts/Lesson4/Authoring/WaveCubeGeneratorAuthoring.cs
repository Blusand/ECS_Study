using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson4
{
    struct WaveCubeGenerator : IComponentData
    {
        public Entity CubeProtoType;
        public int XHalfCount;
        public int ZHalfCount;
    }

    public class WaveCubeGeneratorAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject m_CubePrefab = null;
        [SerializeField, Range(10, 100)] private int m_XHalfCount = 40;
        [SerializeField, Range(10, 100)] private int m_ZHalfCount = 40;

        class Baker : Baker<WaveCubeGeneratorAuthoring>
        {
            public override void Bake(WaveCubeGeneratorAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new WaveCubeGenerator
                {
                    CubeProtoType = GetEntity(authoring.m_CubePrefab, TransformUsageFlags.Dynamic),
                    XHalfCount = authoring.m_XHalfCount,
                    ZHalfCount = authoring.m_ZHalfCount
                });
            }
        }
    }
}