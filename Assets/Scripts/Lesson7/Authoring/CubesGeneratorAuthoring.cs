using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Entities.Lesson7
{
    struct CubesGeneratorData : IComponentData
    {
        public Entity CubeProtoType;
        public int GenerationTotalNum;
        public int GenerationNumPerTickTime;
        public float TickTime;
        public float3 GeneratorAreaPos;
        public float3 GeneratorAreaSize;
        public float3 TargetAreaPos;
        public float3 TargetAreaSize;
        public float RotateSpeed;
        public float MoveSpeed;
    }

    public class CubesGeneratorAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject m_CubePrefab = null;
        [SerializeField, Range(10, 10000)] private int GenerationTotalNum = 500;
        [SerializeField, Range(1, 60)] private int GenerationNumPerTickTime = 5;
        [SerializeField, Range(0.1f, 1.0f)] private float TickTime = 0.2f;
        [SerializeField] private float3 GeneratorAreaPos;
        [SerializeField] private float3 GeneratorAreaSize;
        [SerializeField] private float3 TargetAreaPos;
        [SerializeField] private float3 TargetAreaSize;
        [SerializeField] private float RotateSpeed = 180.0f;
        [SerializeField] private float MoveSpeed = 5.0f;

        public class Baker : Baker<CubesGeneratorAuthoring>
        {
            public override void Bake(CubesGeneratorAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new CubesGeneratorData
                {
                    CubeProtoType = GetEntity(authoring.m_CubePrefab, TransformUsageFlags.Dynamic),
                    GenerationTotalNum = authoring.GenerationTotalNum,
                    GenerationNumPerTickTime = authoring.GenerationNumPerTickTime,
                    TickTime = authoring.TickTime,
                    GeneratorAreaPos = authoring.GeneratorAreaPos,
                    GeneratorAreaSize = authoring.GeneratorAreaSize,
                    TargetAreaPos = authoring.TargetAreaPos,
                    TargetAreaSize = authoring.TargetAreaSize,
                    RotateSpeed = authoring.RotateSpeed,
                    MoveSpeed = authoring.MoveSpeed
                });
            }
        }
    }
}