using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Entities.Lesson8
{
    struct MultiCubesGeneratorData : IComponentData
    {
        public Entity RedCubeProtoEntity;
        public Entity GreenCubeProtoEntity;
        public Entity BlueCubeProtoEntity;
        public int GenerationTotalNum;
        public int GenerationPerTickTime;
        public float TickTime;
        public float3 RedCubeGeneratorPos;
        public float3 BlueCubeGeneratorPos;
        public float3 GreenCubeGeneratorPos;
        public float3 CubeTargetPos;
    }

    public class MultiCubesGeneratorAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject m_RedCubePrefab = null;
        [SerializeField] private GameObject m_GreenCubePrefab = null;
        [SerializeField] private GameObject m_BlueCubePrefab = null;
        [SerializeField, Range(10, 10000)] private int m_GenerationTotalNum = 500;
        [SerializeField, Range(1, 60)] private int m_GenerationNumPerTickTime = 5;
        [SerializeField, Range(0.1f, 1.0f)] private float m_TickTime = 0.2f;
        [SerializeField] private float3 m_RedCubeGeneratorPos;
        [SerializeField] private float3 m_BlueCubeGeneratorPos;
        [SerializeField] private float3 m_GreenCubeGeneratorPos;
        [SerializeField] private float3 m_CubeTargetPos;

        public class Baker : Baker<MultiCubesGeneratorAuthoring>
        {
            public override void Bake(MultiCubesGeneratorAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new MultiCubesGeneratorData
                {
                    RedCubeProtoEntity = GetEntity(authoring.m_RedCubePrefab),
                    GreenCubeProtoEntity = GetEntity(authoring.m_GreenCubePrefab),
                    BlueCubeProtoEntity = GetEntity(authoring.m_BlueCubePrefab),
                    GenerationTotalNum = authoring.m_GenerationTotalNum,
                    GenerationPerTickTime = authoring.m_GenerationNumPerTickTime,
                    TickTime = authoring.m_TickTime,
                    RedCubeGeneratorPos = authoring.m_RedCubeGeneratorPos,
                    GreenCubeGeneratorPos = authoring.m_GreenCubeGeneratorPos,
                    BlueCubeGeneratorPos = authoring.m_BlueCubeGeneratorPos,
                    CubeTargetPos = authoring.m_CubeTargetPos,
                });
            }
        }
    }
}