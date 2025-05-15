using Entities.Lesson3;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Entities.Lesson5
{
    [BurstCompile]
    struct GenerateCubesJob : IJobFor
    {
        [ReadOnly] public Entity CubeProtoType;
        public NativeArray<Entity> Cubes;
        public EntityCommandBuffer Ecb;
        [NativeDisableUnsafePtrRestriction] public RefRW<RandomSingletonData> Random;

        public void Execute(int index)
        {
            Cubes[index] = Ecb.Instantiate(CubeProtoType);
            Ecb.AddComponent<RotateAndMoveSpeedData>(Cubes[index], new RotateAndMoveSpeedData
            {
                RotateSpeed = math.radians(60.0f),
                MoveSpeed = 5.0f
            });

            float2 targetPos2D = Random.ValueRW.Random.NextFloat2(new float2(-15, -15), new float2(15, 15));
            Ecb.AddComponent(Cubes[index], new RandomTargetData
            {
                TargetPos = new float3(targetPos2D.x, 0, targetPos2D.y)
            });
        }
    }
}