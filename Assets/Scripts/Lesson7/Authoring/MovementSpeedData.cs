using Unity.Entities;

namespace Entities.Lesson7
{
    public struct MovementSpeedData : IComponentData, IEnableableComponent
    {
        public float MovementSpeed;
    }
}