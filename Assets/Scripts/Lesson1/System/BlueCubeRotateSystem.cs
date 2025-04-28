using Entities.Lesson0;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Entities.Lesson1
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(RotateCubesFilterSystemGroup))]
    [BurstCompile]
    public partial struct BlueCubeRotateSystem : ISystem
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
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (transform, speed, tag) in SystemAPI
                         .Query<RefRW<LocalTransform>, RefRO<RotateSpeedData>, RefRO<BlueCubeTag>>())
            {
                transform.ValueRW = transform.ValueRO.RotateY(speed.ValueRO.RotateSpeed * deltaTime);
            }
        }
    }
}