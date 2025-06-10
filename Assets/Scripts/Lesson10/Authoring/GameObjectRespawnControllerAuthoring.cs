using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson10
{
    class GameObjectRespawnControllerData : IComponentData
    {
        public float Timer;
        public GameObject[] Prefabs;
    }

    public class GameObjectRespawnControllerAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_Spawners = null;
        [SerializeField, Range(1, 5)] private float m_Timer = 3.0f;

        public class Baker : Baker<GameObjectRespawnControllerAuthoring>
        {
            public override void Bake(GameObjectRespawnControllerAuthoring authoring)
            {
                AddComponentObject(GetEntity(TransformUsageFlags.None), new GameObjectRespawnControllerData
                {
                    Timer = authoring.m_Timer,
                    Prefabs = authoring.m_Spawners
                });
            }
        }
    }
}