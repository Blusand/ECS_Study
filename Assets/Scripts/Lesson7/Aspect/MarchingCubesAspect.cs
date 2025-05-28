using Entities.Lesson5;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson7
{
    public readonly partial struct MarchingCubesAspect : IAspect
    {
        private readonly RefRW<LocalTransform> m_LocalTransform;
        private readonly RefRO<MovementSpeedData> m_MovementSpeed;
        private readonly RefRO<RandomTargetData> m_TargetPos;

        public bool IsNeedDestroy()
        {
            var distance = math.distance(m_LocalTransform.ValueRO.Position, m_TargetPos.ValueRO.TargetPos);
            if (distance <= 0.02f)
            {
                return true;
            }

            return false;
        }

        public void Move(float deltaTime)
        {
            float3 dir = math.normalize(m_TargetPos.ValueRO.TargetPos - m_LocalTransform.ValueRO.Position);
            m_LocalTransform.ValueRW.Position += dir * deltaTime * m_MovementSpeed.ValueRO.MovementSpeed;
        }
    }
}