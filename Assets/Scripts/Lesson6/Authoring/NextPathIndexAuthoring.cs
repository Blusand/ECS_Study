using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson6
{
    struct NextPathIndexData : IComponentData
    {
        public uint NextIndex;
    }

    public class NextPathIndexAuthoring : MonoBehaviour
    {
        [HideInInspector] public uint m_NextIndex = 0;

        public class Baker : Baker<NextPathIndexAuthoring>
        {
            public override void Bake(NextPathIndexAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new NextPathIndexData
                {
                    NextIndex = authoring.m_NextIndex
                });
            }
        }
    }
}