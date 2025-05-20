using Entities.Base;

namespace Entities.Lesson5
{
    public partial class RandomGenerateCubesSystemGroup : AuthoringSceneSystemGroup
    {
        protected override string AuthoringSceneName => "RandomGenerateCubes";
    }

    public partial class RandomGenerateCubesWithParallelWriterSystemGroup : AuthoringSceneSystemGroup
    {
        protected override string AuthoringSceneName => "RandomGenerateCubesWithParallelWriter";
    }
}