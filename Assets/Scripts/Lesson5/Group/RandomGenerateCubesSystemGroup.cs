using Entities.Base;

namespace Lesson5.Group
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