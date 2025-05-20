using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Entities.Lesson6
{
    [BurstCompile]
    [UpdateInGroup(typeof(MoveCubesWithWayPointsSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    [UpdateAfter(typeof(MoveCubesWithWayPointsSystem))]
    public partial struct InputSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<WayPointData>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        // [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var path = SystemAPI.GetSingletonBuffer<WayPointData>();
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float3 newWayPoint = new float3(worldPos.x, worldPos.y, 0);
                if (path.Length > 0)
                {
                    float minDist = float.MaxValue;
                    int index = path.Length;
                    // 找到插入位置
                    for (int i = 0; i < path.Length; i++)
                    {
                        float dist = math.distance(path[i].Point, newWayPoint);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            index = i;
                        }
                    }

                    path.Insert(index, new WayPointData { Point = newWayPoint });
                }
            }
        }
    }
}