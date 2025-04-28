using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson1
{
    struct GreenCubeTag : IComponentData
    {
    }

    public class GreenTagAuthoring : MonoBehaviour
    {
        public class Baker : Baker<GreenTagAuthoring>
        {
            public override void Bake(GreenTagAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new GreenCubeTag());
            }
        }
    }
}