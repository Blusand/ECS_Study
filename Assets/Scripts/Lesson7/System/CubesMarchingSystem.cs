using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Profiling;
using Unity.Transforms;

namespace Entities.Lesson7
{
    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(CubesMarchSystemGroup))]
    [UpdateAfter(typeof(CubesGeneratorSystem))]
    public partial struct CubesMarchingSystem : ISystem
    {
        private static readonly ProfilerMarker m_ProfilerMarker = new ProfilerMarker("CubesMarchingWithEntity");
        private EntityQuery m_CubesQuery;
        private ComponentTypeHandle<LocalTransform> m_LocalTransformTypeHandle;
        private ComponentTypeHandle<RotateSpeedData> m_RotateSpeedTypeHandle;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MovementSpeedData>();
            state.RequireForUpdate<RotateSpeedData>();

            var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<RotateSpeedData, LocalTransform>()
                .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
            m_CubesQuery = state.GetEntityQuery(queryBuilder);

            m_LocalTransformTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
            m_RotateSpeedTypeHandle = state.GetComponentTypeHandle<RotateSpeedData>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            using (m_ProfilerMarker.Auto())
            {
                m_LocalTransformTypeHandle.Update(ref state);
                m_RotateSpeedTypeHandle.Update(ref state);

                var generator = SystemAPI.GetSingleton<CubesGeneratorData>();
                var job0 = new StopCubesRotateChunkJob
                {
                    DeltaTime = SystemAPI.Time.DeltaTime,
                    ElapsedTime = (float)SystemAPI.Time.ElapsedTime,
                    LeftRightBound = new float2(generator.GeneratorAreaPos.x / 2, generator.GeneratorAreaSize.x / 2),
                    TransformTypeHandle = m_LocalTransformTypeHandle,
                    RotateSpeedTypeHandle = m_RotateSpeedTypeHandle
                };
                state.Dependency = job0.ScheduleParallel(m_CubesQuery, state.Dependency);

                var ecb = new EntityCommandBuffer(Allocator.TempJob);
                var ecbParallel = ecb.AsParallelWriter();
                var job1 = new CubesMarchingEntityJob
                {
                    DeltaTime = SystemAPI.Time.DeltaTime,
                    EcbParallel = ecbParallel
                };
                state.Dependency = job1.ScheduleParallel(state.Dependency);
                state.Dependency.Complete();
                ecb.Playback(state.EntityManager);
                ecb.Dispose();
            }
        }
    }
}