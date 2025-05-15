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
    partial struct GenerateCubesWithParallelWriterJob : IJobFor
    {
        [ReadOnly] public Entity CubeProtoType;
        public NativeArray<Entity> Cubes;

        public EntityCommandBuffer.ParallelWriter EcbParallel;

        // 在ScheduleParallel调度下一般是禁止使用RefRW引用读写的，因为
        // 有可能会发生race condition（竞争条件）的情况，
        // 但由于我们很明确全局只会有一个RandomSingleton对象，并且生成的
        // 随机数是线程安全的，所以我们要在前面加上NativeDisableUnsafePtrRestriction属性修饰。
        // 这样就不会触发job的安全性检查了
        [NativeDisableUnsafePtrRestriction] public RefRW<RandomSingletonData> Random;

        public void Execute(int index)
        {
            Cubes[index] = EcbParallel.Instantiate(index, CubeProtoType);
            EcbParallel.AddComponent<RotateAndMoveSpeedData>(index, Cubes[index], new RotateAndMoveSpeedData
            {
                RotateSpeed = math.radians(60.0f),
                MoveSpeed = 5.0f
            });

            float2 targetPos2D = Random.ValueRW.Random.NextFloat2(new float2(-15, -15), new float2(15, 15));
            EcbParallel.AddComponent(index, Cubes[index], new RandomTargetData
            {
                TargetPos = new float3(targetPos2D.x, 0, targetPos2D.y)
            });
        }
    }
}