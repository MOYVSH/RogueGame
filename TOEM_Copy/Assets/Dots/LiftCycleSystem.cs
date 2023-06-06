using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[RequireMatchingQueriesForUpdate]
[BurstCompile]
public partial struct LiftCycleSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DirectoryManaged>();
        state.RequireForUpdate<LiftCycleData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        double EndTime = SystemAPI.Time.ElapsedTime;
        foreach (var (liftCycleData, entity) in SystemAPI.Query<RefRO<LiftCycleData>>().WithEntityAccess())
        {
            if ((float)(EndTime - liftCycleData.ValueRO.StartTime) >= liftCycleData.ValueRO.LiftTime)
            {
                ecb.DestroyEntity(entity);
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

public struct LiftCycleData : IComponentData
{
    public float LiftTime;
    public double StartTime;
}
