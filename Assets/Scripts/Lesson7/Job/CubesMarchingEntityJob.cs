using Unity.Burst;
using Unity.Entities;

namespace Entities.Lesson7
{
    [BurstCompile]
    partial struct CubesMarchingEntityJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter EcbParallel;

        private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity, MarchingCubesAspect aspect)
        {
            if (aspect.IsNeedDestroy())
            {
                EcbParallel.DestroyEntity(chunkIndex, entity);
            }
            else
            {
                aspect.Move(DeltaTime);
            }
        }
    }
}