using Entities.Lesson0;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Entities.Lesson2
{
    [BurstCompile]
    partial struct RotateCubeWithJobChunk : IJobChunk
    {
        public float DeltaTime;

        public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
        public ComponentTypeHandle<RotateSpeedData> RotateSpeedTypeHandle;

        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask,
            in v128 chunkEnabledMask)
        {
            var chunkTransforms = chunk.GetNativeArray(ref TransformTypeHandle);
            var chunkRotateSpeeds = chunk.GetNativeArray(ref RotateSpeedTypeHandle);

            // 第一种方式：通过ChunkEntity枚举器的方式遍历
            var enumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
            while (enumerator.NextEntityIndex(out var idx))
            {
                var speed = chunkRotateSpeeds[idx];
                chunkTransforms[idx] = chunkTransforms[idx].RotateY(speed.RotateSpeed * DeltaTime);
            }

            // 第二种方式：直接遍历方式，需要手动过滤掉不要的数据
            // for (int i = 0; i < chunk.Count; i++)
            // {
            //     var speed = chunkRotateSpeeds[i];
            //     chunkTransforms[i] = chunkTransforms[i].RotateY(speed.RotateSpeed * DeltaTime);
            // }
        }
    }

    [BurstCompile]
    [UpdateInGroup(typeof(CubeRotateWithIJobChunkSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct CubeRotateWithIJobChunkSystem : ISystem
    {
        private EntityQuery m_RotateCubes;
        private ComponentTypeHandle<LocalTransform> m_LocalTransformTypeHandle;
        private ComponentTypeHandle<RotateSpeedData> m_RotateSpeedTypeHandle;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<RotateSpeedData, LocalTransform>();
            // 在Job中需要访问的数据
            m_RotateCubes = state.GetEntityQuery(queryBuilder);

            // 标识Job直接访问的数据类型
            m_LocalTransformTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
            m_RotateSpeedTypeHandle = state.GetComponentTypeHandle<RotateSpeedData>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            m_LocalTransformTypeHandle.Update(ref state);
            m_RotateSpeedTypeHandle.Update(ref state);

            var job = new RotateCubeWithJobChunk
            {
                TransformTypeHandle = m_LocalTransformTypeHandle,
                RotateSpeedTypeHandle = m_RotateSpeedTypeHandle,
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            state.Dependency = job.ScheduleParallel(m_RotateCubes, state.Dependency);
        }
    }
}