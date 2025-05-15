using Entities.Base;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace Entities.Lesson5
{
    public struct RandomSingletonData : IComponentData
    {
        public Random Random;
    }

    public class RandomSingletonAuthoring : Singleton<RandomSingletonAuthoring>
    {
        public uint Seed;

        public class RandomSingletonDataBaker : Baker<RandomSingletonAuthoring>
        {
            public override void Bake(RandomSingletonAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new RandomSingletonData
                {
                    Random = new Random(Instance.Seed)
                });
            }
        }
    }
}