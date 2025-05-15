using Entities.Lesson5;
using Lesson5.Group;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Lesson5.System
{
    [BurstCompile]
    [UpdateInGroup(typeof(RandomGenerateCubesSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct RandomCubeGenerateSystem : ISystem
    {
        private float m_Timer;
        private int m_TotalCubes;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RandomCubeGeneratorData>();
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
            var generator = SystemAPI.GetSingleton<RandomCubeGeneratorData>();
            if (m_TotalCubes >= generator.GenerationTotalNum)
            {
                state.Enabled = false;
                return;
            }

            if (m_Timer >= generator.TickTime)
            {
                CreateGenerateCubesEntityJob(ref state, generator);
                m_Timer -= generator.TickTime;
            }

            m_Timer += SystemAPI.Time.DeltaTime;
        }

        private void CreateGenerateCubesEntityJob(ref SystemState state, RandomCubeGeneratorData generator)
        {
            // 这里的RandomSingleton如果不是引用的话，而是直接赋值给GenerateCubesWithParallelWriterJob，
            // 那么random就会进行值拷贝，导致在job中每次获得的随机数都相同
            var random = SystemAPI.GetSingletonRW<RandomSingletonData>();
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var cubes = CollectionHelper.CreateNativeArray<Entity>(generator.GenerationNumPerTick, Allocator.TempJob);
            if (generator.UseScheduleParallel)
            {
                var ecbParallel = ecb.AsParallelWriter();
                var job = new GenerateCubesWithParallelWriterJob
                {
                    CubeProtoType = generator.CubeProtoType,
                    Cubes = cubes,
                    EcbParallel = ecbParallel,
                    Random = random
                };
                state.Dependency = job.ScheduleParallel(cubes.Length, 1, state.Dependency);
            }
            else
            {
                var job = new GenerateCubesJob
                {
                    CubeProtoType = generator.CubeProtoType,
                    Cubes = cubes,
                    Ecb = ecb,
                    Random = random
                };
                state.Dependency = job.Schedule(cubes.Length, state.Dependency);
            }

            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            m_TotalCubes += generator.GenerationNumPerTick;
            cubes.Dispose();
            ecb.Dispose();
        }
    }
}