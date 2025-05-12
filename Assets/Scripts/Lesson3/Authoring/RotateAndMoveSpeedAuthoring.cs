using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson3
{
    struct RotateAndMoveSpeedData : IComponentData
    {
        public float RotateSpeed;
        public float MoveSpeed;
    }

    public class RotateAndMoveSpeedAuthoring : MonoBehaviour
    {
        [SerializeField, Range(0, 360)] public float m_RotateSpeed = 360.0f;
        [SerializeField, Range(0, 10)] public float m_MoveSpeed = 1.0f;

        public class Baker : Baker<RotateAndMoveSpeedAuthoring>
        {
            public override void Bake(RotateAndMoveSpeedAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new RotateAndMoveSpeedData
                {
                    RotateSpeed = authoring.m_RotateSpeed,
                    MoveSpeed = authoring.m_MoveSpeed
                });
            }
        }
    }
}