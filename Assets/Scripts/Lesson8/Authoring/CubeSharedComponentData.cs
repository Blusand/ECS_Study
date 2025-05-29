using Unity.Entities;

namespace Entities.Lesson8
{
    public struct CubeSharedComponentData : ISharedComponentData
    {
        public float RotateSpeed;
        public float MoveSpeed;
    }

    public struct SharingGroup : ISharedComponentData
    {
        // 0 red，1 green，2 blue
        public int Group;
    }

    public struct CubeComponentData : IComponentData
    {
        public float RotateSpeed;
        public float MoveSpeed;
    }
}