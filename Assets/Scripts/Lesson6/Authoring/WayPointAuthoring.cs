using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Entities.Lesson6
{
    [InternalBufferCapacity(8)]
    struct WayPointData : IBufferElementData
    {
        public float3 Point;
    }

    public class WayPointAuthoring : MonoBehaviour
    {
        [SerializeField] private List<Vector3> m_WayPoints = null;

        public class Baker : Baker<WayPointAuthoring>
        {
            public override void Bake(WayPointAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var wayPoints = AddBuffer<WayPointData>(entity);
                wayPoints.Length = authoring.m_WayPoints.Count;
                for (int i = 0; i < authoring.m_WayPoints.Count; i++)
                {
                    wayPoints[i] = new WayPointData
                    {
                        Point = new float3(authoring.m_WayPoints[i])
                    };
                }
            }
        }

        private void Update()
        {
            // 鼠标左键按下
            if (Input.GetMouseButtonDown(0))
            {
                // 把鼠标的屏幕坐标转换为世界坐标
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float3 newWayPoint = new float3(worldPos.x, worldPos.y, 0);
                if (m_WayPoints.Count > 0)
                {
                    float minDist = float.MaxValue;
                    int index = m_WayPoints.Count;
                    // 找到插入位置
                    for (int i = 0; i < m_WayPoints.Count; i++)
                    {
                        // 计算每个点与newWayPoint之间的距离
                        float dist = Vector3.Distance(m_WayPoints[i], newWayPoint);
                        // 找到了一个距离newWayPoint更近的点
                        if (dist < minDist)
                        {
                            minDist = dist;
                            index = i;
                        }
                    }

                    // 将newWayPoint插入到距离wayPoints的最近的点的对应位置中
                    m_WayPoints.Insert(index, new Vector3(newWayPoint.x, newWayPoint.y, newWayPoint.z));
                }
            }
        }

        // 绘制线条
        private void OnDrawGizmos()
        {
            if (m_WayPoints.Count >= 2)
            {
                for (int i = 0; i < m_WayPoints.Count; i++)
                {
                    // 在每两个点之间绘制一条黄色直线，所有的点都要连起来
                    // %是为了让最后一个点与第一个点相连，防止越界
                    Gizmos.color = Color.yellow;
                    // -new Vector3(0, 0, 1)：为了让绘制的线条显示在正方体前面
                    Gizmos.DrawLine(m_WayPoints[i] - Vector3.forward,
                        m_WayPoints[(i + 1) % m_WayPoints.Count] - Vector3.forward);

                    // 在每个点的位置处绘制一个蓝色的圆形
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(m_WayPoints[i] - Vector3.forward, 0.4f);
                }
            }
        }
    }
}