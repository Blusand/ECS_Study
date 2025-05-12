using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Entities.Lesson3
{
    readonly partial struct RotateAndMoveAspect : IAspect
    {
        private readonly RefRW<LocalTransform> m_LocalTransform;
        private readonly RefRO<RotateAndMoveSpeedData> m_Speed;

        public void Move(double elapsedTime)
        {
            m_LocalTransform.ValueRW.Position.y = (float)math.sin(elapsedTime * m_Speed.ValueRO.MoveSpeed);
        }

        public void Rotate(float deltaTime)
        {
            m_LocalTransform.ValueRW = m_LocalTransform.ValueRO.RotateY(deltaTime * m_Speed.ValueRO.RotateSpeed);
        }

        public void RotateAndMove(double elapsedTime, float deltaTime)
        {
            Move(elapsedTime);
            Rotate(deltaTime);
        }
    }
}