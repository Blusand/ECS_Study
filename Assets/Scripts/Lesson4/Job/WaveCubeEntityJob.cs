using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson4
{
    [BurstCompile]
    partial struct WaveCubeEntityJob : IJobEntity
    {
        [ReadOnly] public float ElapsedTime;

        void Execute(ref LocalTransform transform)
        {
            var distance = math.distance(transform.Position, float3.zero);
            transform.Position += new float3(0, 1, 0) * math.sin(ElapsedTime * 3f + distance * 0.2f);
        }
    }
}
