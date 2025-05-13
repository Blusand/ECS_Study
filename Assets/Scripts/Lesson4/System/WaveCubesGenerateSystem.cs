using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson4
{
    [BurstCompile]
    [UpdateInGroup(typeof(WaveCubesWithDotsSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct WaveCubesGenerateSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // 在加载WaveCubeGenerator之前，不应该运行此系统
            state.RequireForUpdate<WaveCubeGenerator>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var generator = SystemAPI.GetSingleton<WaveCubeGenerator>();
            var cubes = CollectionHelper.CreateNativeArray<Entity>(4 * generator.XHalfCount * generator.ZHalfCount,
                Allocator.Temp);
            state.EntityManager.Instantiate(generator.CubeProtoType, cubes);

            int count = 0;
            foreach (var cube in cubes)
            {
                int x = count % (generator.XHalfCount * 2) - generator.XHalfCount;
                int z = count / (generator.XHalfCount * 2) - generator.ZHalfCount;
                var position = new float3(x * 1.1f, 0f, z * 1.1f);

                var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
                transform.ValueRW.Position = position;
                ++count;
            }

            cubes.Dispose();
            // 此System只在启动时运行一次，所以在第一次更新后关闭它
            state.Enabled = false;
        }
    }
}