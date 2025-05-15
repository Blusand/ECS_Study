using Entities.Lesson3;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson5
{
    [BurstCompile]
    partial struct CubeRotateAndMoveEntityJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter EcbParallelWriter;

        void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, ref LocalTransform transform,
            in RandomTargetData target, in RotateAndMoveSpeedData speed)
        {
            var distance = math.distance(transform.Position, target.TargetPos);
            if (distance < 0.02f)
            {
                EcbParallelWriter.DestroyEntity(chunkIndex, entity);
            }
            else
            {
                float3 dir = math.normalize(target.TargetPos - transform.Position);
                transform.Position += dir * speed.MoveSpeed * DeltaTime;
                transform = transform.RotateY(speed.RotateSpeed * DeltaTime);
            }
        }
    }
}