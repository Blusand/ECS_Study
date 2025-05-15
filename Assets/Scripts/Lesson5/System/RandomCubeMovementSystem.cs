using Entities.Lesson3;
using Entities.Lesson5;
using Lesson5.Group;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Lesson5.System
{
    [BurstCompile]
    [UpdateInGroup(typeof(RandomGenerateCubesSystemGroup))]
    [UpdateAfter(typeof(RandomCubeGenerateSystem))]
    [RequireMatchingQueriesForUpdate]
    public partial struct RandomCubeMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RotateAndMoveSpeedData>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var ecbParallel = ecb.AsParallelWriter();
            var job = new CubeRotateAndMoveEntityJob
            {
                EcbParallelWriter = ecbParallel,
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            state.Dependency = job.ScheduleParallel(state.Dependency);
            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}