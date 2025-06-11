using Entities.Lesson11;
using Unity.Burst;
using Unity.Entities;

namespace Lesson11.System
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(ChunkComponentSystemGroup))]
    public partial struct GenerateEntitySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // 如果测试这段代码请开启下面代码
            var entity1 = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData<GeneralComponent>(entity1, new GeneralComponent
            {
                Num = 0
            });
            state.EntityManager.AddChunkComponentData<ChunkComponentA>(entity1);

            var entity2 = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData<GeneralComponent>(entity2, new GeneralComponent
            {
                Num = 0
            });
            state.EntityManager.AddChunkComponentData<ChunkComponentA>(entity2);
            state.EntityManager.AddChunkComponentData<ChunkComponentB>(entity2);

            ArchetypeChunk chunk = state.EntityManager.GetChunk(entity1);
            state.EntityManager.SetChunkComponentData(chunk, new ChunkComponentA
            {
                NumA = 3
            });

            var entity3 = state.EntityManager.CreateEntity();
            state.EntityManager.AddChunkComponentData<ChunkComponentA>(entity3);
            state.EntityManager.AddChunkComponentData<ChunkComponentB>(entity3);

            var entity4 = state.EntityManager.CreateEntity();
            state.EntityManager.AddChunkComponentData<ChunkComponentAB>(entity4);

            state.Enabled = false;
        }
    }
}