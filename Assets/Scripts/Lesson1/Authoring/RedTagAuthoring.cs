using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson1
{
    struct RedCubeTag : IComponentData
    {
    }

    public class RedTagAuthoring : MonoBehaviour
    {
        public class Baker : Baker<RedTagAuthoring>
        {
            public override void Bake(RedTagAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new RedCubeTag());
            }
        }
    }
}