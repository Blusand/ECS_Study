using Entities.Lesson3;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson6
{
    [BurstCompile]
    [UpdateInGroup(typeof(MoveCubesWithWayPointsSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct MoveCubesWithWayPointsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<WayPointData>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var path = SystemAPI.GetSingletonBuffer<WayPointData>();
            float deltaTime = SystemAPI.Time.DeltaTime;
            if (!path.IsEmpty)
            {
                foreach (var (transform, nextIndex, speed) in SystemAPI
                             .Query<RefRW<LocalTransform>, RefRW<NextPathIndexData>, RefRO<RotateAndMoveSpeedData>>())
                {
                    float3 direction = path[(int)nextIndex.ValueRO.NextIndex].Point - transform.ValueRO.Position;
                    transform.ValueRW.Position = transform.ValueRO.Position
                                                 + math.normalize(direction) * speed.ValueRO.MoveSpeed * deltaTime;
                    transform.ValueRW = transform.ValueRO.RotateY(speed.ValueRO.RotateSpeed * deltaTime);

                    // 到达目标点
                    if (math.distance(path[(int)nextIndex.ValueRO.NextIndex].Point, transform.ValueRO.Position)
                        <= 0.02f)
                    {
                        nextIndex.ValueRW.NextIndex = (uint)((nextIndex.ValueRO.NextIndex + 1) % path.Length);
                    }
                }
            }
        }
    }
}