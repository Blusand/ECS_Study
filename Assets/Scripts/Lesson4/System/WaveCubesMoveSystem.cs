using Unity.Burst;
using Unity.Entities;
using Unity.Profiling;

namespace Entities.Lesson4
{
    [BurstCompile]
    [UpdateInGroup(typeof(WaveCubesWithDotsSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct WaveCubesMoveSystem : ISystem
    {
        private static readonly ProfilerMarker m_ProfilerMarker = new ProfilerMarker("WaveCubeEntityJobs");

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
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
                var job = new WaveCubeEntityJob
                {
                    ElapsedTime = (float)SystemAPI.Time.ElapsedTime
                };
                job.ScheduleParallel();
            }
        }
    }
}