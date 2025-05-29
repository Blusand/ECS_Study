using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson8
{
    [BurstCompile]
    [UpdateInGroup(typeof(MultiCubesMarchSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct MultiCubesGenerateSystem : ISystem
    {
        private float m_Timer;
        private int m_TotalCubes;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MultiCubesGeneratorData>();
            m_Timer = 0f;
            m_TotalCubes = 0;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var generator = SystemAPI.GetSingleton<MultiCubesGeneratorData>();
            if (m_TotalCubes >= generator.GenerationTotalNum)
            {
                state.Enabled = false;
                return;
            }

            if (m_Timer >= generator.TickTime)
            {
                Entity redCube = state.EntityManager.Instantiate(generator.RedCubeProtoEntity);
                Entity greenCube = state.EntityManager.Instantiate(generator.GreenCubeProtoEntity);
                Entity blueCube = state.EntityManager.Instantiate(generator.BlueCubeProtoEntity);

                //state.EntityManager.AddComponentData<CubeComponentData>(redCube, new CubeComponentData
                state.EntityManager.AddSharedComponent<CubeSharedComponentData>(redCube, new CubeSharedComponentData
                {
                    RotateSpeed = math.radians(180.0f),
                    MoveSpeed = 5.0f,
                    //MoveSpeed = Random.CreateFromIndex((uint)m_TotalCubes).NextFloat(5.0f, 20.0f)
                });

                state.EntityManager.AddSharedComponent<SharingGroup>(redCube, new SharingGroup
                {
                    Group = 0
                });

                //state.EntityManager.AddComponentData<CubeComponentData>(greenCube, new CubeComponentData
                state.EntityManager.AddSharedComponent<CubeSharedComponentData>(greenCube, new CubeSharedComponentData
                {
                    RotateSpeed = math.radians(180.0f),
                    MoveSpeed = 5.0f,
                    //MoveSpeed = Random.CreateFromIndex((uint)m_TotalCubes).NextFloat(5.0f, 20.0f)
                });

                state.EntityManager.AddSharedComponent<SharingGroup>(greenCube, new SharingGroup
                {
                    Group = 1
                });

                //state.EntityManager.AddComponentData<CubeComponentData>(blueCube, new CubeComponentData
                state.EntityManager.AddSharedComponent<CubeSharedComponentData>(blueCube, new CubeSharedComponentData
                {
                    RotateSpeed = math.radians(180.0f),
                    MoveSpeed = 5.0f,
                    //MoveSpeed = Random.CreateFromIndex((uint)m_TotalCubes).NextFloat(5.0f, 20.0f)
                });

                state.EntityManager.AddSharedComponent<SharingGroup>(blueCube, new SharingGroup
                {
                    Group = 2
                });

                var redCubeTransform = SystemAPI.GetComponentRW<LocalTransform>(redCube);
                redCubeTransform.ValueRW.Position = generator.RedCubeGeneratorPos;

                var greenCubeTransform = SystemAPI.GetComponentRW<LocalTransform>(greenCube);
                greenCubeTransform.ValueRW.Position = generator.GreenCubeGeneratorPos;

                var blueCubeTransform = SystemAPI.GetComponentRW<LocalTransform>(blueCube);
                blueCubeTransform.ValueRW.Position = generator.BlueCubeGeneratorPos;

                m_TotalCubes += 3;
                m_Timer -= generator.TickTime;
            }

            m_Timer += SystemAPI.Time.DeltaTime;
        }
    }
}