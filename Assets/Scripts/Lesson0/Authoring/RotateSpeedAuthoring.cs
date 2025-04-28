using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson0
{
    struct RotateSpeedData : IComponentData
    {
        public float RotateSpeed;
    }

    public class RotateSpeedAuthoring : MonoBehaviour
    {
        [Range(0, 360)] public float RotateSpeed = 360.0f;

        public class Baker : Baker<RotateSpeedAuthoring>
        {
            public override void Bake(RotateSpeedAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new RotateSpeedData
                {
                    RotateSpeed = authoring.RotateSpeed
                });
            }
        }
    }
}