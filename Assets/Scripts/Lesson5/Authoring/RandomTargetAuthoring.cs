using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Entities.Lesson5
{
    public struct RandomTargetData : IComponentData
    {
        public float3 TargetPos;
    }

    public class RandomTargetAuthoring : MonoBehaviour
    {
        public class Baker : Baker<RandomTargetAuthoring>
        {
            public override void Bake(RandomTargetAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new RandomTargetData
                {
                    TargetPos = float3.zero
                });
            }
        }
    }
}