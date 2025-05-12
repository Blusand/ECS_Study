using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson3
{
    [BurstCompile]
    [UpdateInGroup(typeof(CreateEntitiesByPrefabSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CubeRotateAndMoveSystem : ISystem
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
            float deltaTime = SystemAPI.Time.DeltaTime;
            double elapsedTime = SystemAPI.Time.ElapsedTime;

            // 1.Use Component Ref
            // foreach (var (transform, speed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateAndMoveSpeedData>>())
            // {
            //     transform.ValueRW = transform.ValueRO.RotateY(speed.ValueRO.RotateSpeed * deltaTime);
            //     transform.ValueRW.Position.y = (float)math.sin(elapsedTime * speed.ValueRO.MoveSpeed);
            // }

            // 2.Use RotateAndMoveAspect
            foreach (var aspect in SystemAPI.Query<RotateAndMoveAspect>())
            {
                aspect.RotateAndMove(elapsedTime, deltaTime);
            }
        }
    }
}