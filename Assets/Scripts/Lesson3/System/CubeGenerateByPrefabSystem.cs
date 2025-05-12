using Entities.Lesson3;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Lesson3.System
{
    [BurstCompile]
    [UpdateInGroup(typeof(CreateEntitiesByPrefabSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CubeGenerateByPrefabSystem : ISystem
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
            var generator = SystemAPI.GetSingleton<CubeGeneratorByPrefabData>();
            var cubes = CollectionHelper.CreateNativeArray<Entity>(generator.CubeCount, Allocator.Temp);
            state.EntityManager.Instantiate(generator.CubeEntityProtoType, cubes);
            int count = 0;
            foreach (var cube in cubes)
            {
                state.EntityManager.AddComponentData<RotateAndMoveSpeedData>(cube, new RotateAndMoveSpeedData()
                {
                    MoveSpeed = count * math.radians(60.0f),
                    RotateSpeed = count
                });

                var position = new float3((count - generator.CubeCount * 0.5f) * 1.2f, 0f, 0f);
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