using Unity.Entities;

namespace Entities.Lesson7
{
    public struct RotateSpeedData : IComponentData, IEnableableComponent
    {
        public float RotateSpeed;
    }
}