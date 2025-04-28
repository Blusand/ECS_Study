using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Entities.Lesson0
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(CubeRotateSystemGroup))]
    [BurstCompile]
    public partial struct CubeRotateSystem : ISystem
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
            foreach (var (transform, speed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeedData>>())
            {
                transform.ValueRW = transform.ValueRO.RotateY(speed.ValueRO.RotateSpeed * SystemAPI.Time.DeltaTime);
            }
        }
    }
}