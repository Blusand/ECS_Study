using Unity.Entities;

namespace Entities.Lesson11
{
    struct ChunkComponentA : IComponentData
    {
        public int NumA;
    }

    struct ChunkComponentB : IComponentData
    {
        public int NumB;
    }

    struct ChunkComponentAB : IComponentData
    {
        public int NumA;
        public int NumB;
    }
}