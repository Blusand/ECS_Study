using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;

namespace Entities.Lesson9
{
    [BurstCompile]
    [UpdateInGroup(typeof(SpawnerBlobSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct GenerateSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpawnerGeneratorData>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var generator = SystemAPI.GetSingleton<SpawnerGeneratorData>();
            var spawners =
                CollectionHelper.CreateNativeArray<Entity>(4 * generator.HalfCountX * generator.HalfCountZ,
                    Allocator.Temp);
            state.EntityManager.Instantiate(generator.SpawnerProtoType, spawners);

            int count = 0;
            foreach (var cube in spawners)
            {
                int x = count % (generator.HalfCountX * 2) - generator.HalfCountX;
                int z = count / (generator.HalfCountX * 2) - generator.HalfCountZ;
                var position = new float3(x * 1.1f, 0, z * 1.1f);

                var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
                transform.ValueRW.Position = position;
                ++count;
            }

            spawners.Dispose();
            // 此System只在启动时运行一次，所以在第一次更新后关闭它。
            state.Enabled = false;
        }
    }
}