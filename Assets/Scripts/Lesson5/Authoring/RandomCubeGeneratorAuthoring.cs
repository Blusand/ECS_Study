using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson5
{
    struct RandomCubeGeneratorData : IComponentData
    {
        public Entity CubeProtoType;
        public int GenerationTotalNum;
        public int GenerationNumPerTick;
        public float TickTime;
        public bool UseScheduleParallel;
    }

    public class RandomCubeGeneratorAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject m_RedCubePrefab = null;
        [SerializeField] private GameObject m_BlueCubePrefab = null;
        [SerializeField, Range(10, 10000)] public int m_GeneratonTotalNum = 500;
        [SerializeField, Range(1, 60)] public int m_GenerationNumPerTick = 5;
        [SerializeField, Range(0.1f, 1.0f)] public float m_TickTime = 0.2f;
        public bool UseScheduleParallel = false;

        public class Baker : Baker<RandomCubeGeneratorAuthoring>
        {
            public override void Bake(RandomCubeGeneratorAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new RandomCubeGeneratorData
                {
                    CubeProtoType =
                        GetEntity(
                            authoring.UseScheduleParallel ? authoring.m_RedCubePrefab : authoring.m_BlueCubePrefab,
                            TransformUsageFlags.Dynamic),
                    GenerationTotalNum = authoring.m_GeneratonTotalNum,
                    GenerationNumPerTick = authoring.m_GenerationNumPerTick,
                    TickTime = authoring.m_TickTime,
                    UseScheduleParallel = authoring.UseScheduleParallel
                });
            }
        }
    }
}