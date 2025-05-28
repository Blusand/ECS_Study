using Entities.Lesson5;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson7
{
    [BurstCompile]
    [UpdateInGroup(typeof(CubesMarchSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CubesGeneratorSystem : ISystem
    {
        private float m_Timer;
        private int m_TotalCubes;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubesGeneratorData>();
            m_Timer = 0;
            m_TotalCubes = 0;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var generator = SystemAPI.GetSingleton<CubesGeneratorData>();
            if (m_TotalCubes >= generator.GenerationTotalNum)
            {
                state.Enabled = false;
                return;
            }

            if (m_Timer >= generator.TickTime)
            {
                var cubes = CollectionHelper.CreateNativeArray<Entity>(generator.GenerationNumPerTickTime,
                    Allocator.Temp);
                state.EntityManager.Instantiate(generator.CubeProtoType, cubes);
                foreach (var cube in cubes)
                {
                    state.EntityManager.AddComponentData<RotateSpeedData>(cube, new RotateSpeedData
                    {
                        RotateSpeed = math.radians(generator.RotateSpeed)
                    });

                    state.EntityManager.AddComponentData<MovementSpeedData>(cube, new MovementSpeedData
                    {
                        MovementSpeed = generator.MoveSpeed
                    });

                    // 设置随机目标点
                    var randomSingleton = SystemAPI.GetSingletonRW<RandomSingletonData>();
                    var randPos = randomSingleton.ValueRW.Random.NextFloat3(-generator.TargetAreaSize * 0.5f,
                        generator.TargetAreaSize * 0.5f);
                    state.EntityManager.AddComponentData<RandomTargetData>(cube, new RandomTargetData
                    {
                        TargetPos = generator.TargetAreaPos + new float3(randPos.x, 0, randPos.z)
                    });

                    // 设置随机初始点
                    randomSingleton = SystemAPI.GetSingletonRW<RandomSingletonData>();
                    randPos = randomSingleton.ValueRW.Random.NextFloat3(-generator.TargetAreaSize * 0.5f,
                        generator.TargetAreaSize * 0.5f);
                    var position = generator.GeneratorAreaPos + new float3(randPos.x, 0, randPos.z);
                    var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
                    transform.ValueRW.Position = position;
                }

                cubes.Dispose();
                m_TotalCubes += generator.GenerationNumPerTickTime;
                m_Timer -= generator.TickTime;
            }

            m_Timer += SystemAPI.Time.DeltaTime;
        }
    }
}