using Unity.Entities;
using Unity.Transforms;

namespace Entities.Lesson4
{
    readonly partial struct WaveCubeAspect : IAspect
    {
        readonly RefRW<LocalTransform> m_LocalTransform;
    }
}