using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson1
{
    struct BlueCubeTag : IComponentData
    {
    }

    public class BlueTagAuthoring : MonoBehaviour
    {
        public class Baker : Baker<BlueTagAuthoring>
        {
            public override void Bake(BlueTagAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new BlueCubeTag());
            }
        }
    }
}