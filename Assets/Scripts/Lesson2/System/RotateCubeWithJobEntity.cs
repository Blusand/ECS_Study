using Entities.Lesson0;
using Entities.Lesson2;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Entities.Lesson2
{
    partial struct RotateCubeWithJobEntity : IJobEntity
    {
        public float DeltaTime;

        void Execute(ref LocalTransform transform, in RotateSpeedData speed)
        {
            transform = transform.RotateY(DeltaTime * speed.RotateSpeed);
        }
    }

    [BurstCompile]
    [UpdateInGroup(typeof(CubeRotateWithIJobEntitySystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CubeRotateWithIJobEntitySystem : ISystem
    {
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
            var job = new RotateCubeWithJobEntity
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            job.ScheduleParallel();
        }
    }
}