using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson7
{
    [BurstCompile]
    public partial struct StopCubesRotateChunkJob : IJobChunk
    {
        public float DeltaTime;
        public float ElapsedTime;
        public float2 LeftRightBound;
        public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
        public ComponentTypeHandle<RotateSpeedData> RotateSpeedTypeHandle;

        public void Execute(in ArchetypeChunk chunk, int unfilterdChunkIndex, bool useEnabledMask,
            in v128 chunkEnabledMask)
        {
            var chunkTransforms = chunk.GetNativeArray(ref TransformTypeHandle);
            var chunkRotateSpeeds = chunk.GetNativeArray(ref RotateSpeedTypeHandle);
            var enumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
            while (enumerator.NextEntityIndex(out var index))
            {
                bool enabled = chunk.IsComponentEnabled(ref RotateSpeedTypeHandle, index);
                // 激活
                if (enabled)
                {
                    // 停止旋转
                    if (chunkTransforms[index].Position.x > LeftRightBound.x &&
                        chunkTransforms[index].Position.x < LeftRightBound.y)
                    {
                        chunk.SetComponentEnabled(ref RotateSpeedTypeHandle, index, false);
                    }
                    // 旋转
                    else
                    {
                        var speed = chunkRotateSpeeds[index];
                        chunkTransforms[index] = chunkTransforms[index].RotateY(speed.RotateSpeed * DeltaTime);
                    }
                }
                // 未激活
                else
                {
                    // 缩放变为1
                    if (chunkTransforms[index].Position.x < LeftRightBound.x ||
                        chunkTransforms[index].Position.x > LeftRightBound.y)
                    {
                        chunk.SetComponentEnabled(ref RotateSpeedTypeHandle, index, true);
                        var trans = chunkTransforms[index];
                        trans.Scale = 1;
                        chunkTransforms[index] = trans;
                    }
                    // 缩放不断变换
                    else
                    {
                        var trans = chunkTransforms[index];
                        trans.Scale = math.sin(ElapsedTime * 4) * 0.3f + 1.0f;
                        chunkTransforms[index] = trans;
                    }
                }
            }
        }
    }
}