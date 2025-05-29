using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson8
{
    [BurstCompile]
    [UpdateInGroup(typeof(MultiCubesMarchSystemGroup))]
    [UpdateAfter(typeof(MultiCubesGenerateSystem))]
    [RequireMatchingQueriesForUpdate]
    public partial struct MultiCubesMarchingSystem : ISystem
    {
        private EntityQuery m_CubesQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // var queryBuilder = new EntityQueryBuilder(Allocator.Temp)
            //     .WithAll<LocalTransform, CubeComponentData, SharingGroup>();

            var queryBuilder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<LocalTransform, CubeSharedComponentData, SharingGroup>();
            m_CubesQuery = state.GetEntityQuery(queryBuilder);
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
            var generator = SystemAPI.GetSingleton<MultiCubesGeneratorData>();
            m_CubesQuery.SetSharedComponentFilter(new SharingGroup
            {
                Group = 1
            });

            var cubeEntities = m_CubesQuery.ToEntityArray(Allocator.Temp);
            var localTransform = m_CubesQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            for (int i = 0; i < cubeEntities.Length; i++)
            {
                //var data = state.EntityManager.GetComponentData<CubeComponentData>(cubeEntities[i]);
                var data = state.EntityManager.GetSharedComponent<CubeSharedComponentData>(cubeEntities[i]);
                LocalTransform temp = localTransform[i];
                if (temp.Position.x > generator.CubeTargetPos.x)
                {
                    state.EntityManager.DestroyEntity(cubeEntities[i]);
                }
                else
                {
                    temp.Position += data.MoveSpeed * deltaTime * new float3(1, (float)math.sin(elapsedTime * 20), 0);
                    temp = temp.RotateY(data.RotateSpeed * deltaTime);
                    // LocalTransform是一个值类型，修改后需要重新赋值
                    localTransform[i] = temp;
                    state.EntityManager.SetComponentData(cubeEntities[i], localTransform[i]);
                }
            }

            localTransform.Dispose();
            cubeEntities.Dispose();
        }
    }
}