using Entities.Base;

namespace Entities.Lesson2
{
    public partial class CubeRotateWithIJobEntitySystemGroup : AuthoringSceneSystemGroup
    {
        protected override string AuthoringSceneName => "CubeRotateWithIJobEntity";
    }

    public partial class CubeRotateWithIJobChunkSystemGroup : AuthoringSceneSystemGroup
    {
        protected override string AuthoringSceneName => "CubeRotateWithIJobChunk";
    }
}