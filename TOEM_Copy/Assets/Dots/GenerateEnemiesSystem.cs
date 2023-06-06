using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[RequireMatchingQueriesForUpdate]
[BurstCompile]
public partial struct GenerateEnemiesSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DirectoryManaged>();
        state.RequireForUpdate<GenerateEnemiesData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        //此系统所在的世界中未被管理的部分。
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        DirectoryManaged directory = SystemAPI.ManagedAPI.GetSingleton<DirectoryManaged>();

        if (directory._GameManager.Count >= 500)
        {
            return;
        }

        var job = new GenerateHostile
        {
            LocalTransformLookup =
                SystemAPI.GetComponentLookup<LocalTransform>(true),
            ECB = ecb,
            StartTime = SystemAPI.Time.ElapsedTime
        };

        job.Schedule();
        directory._GameManager.Count++;
    }

    private LocalToWorld RandomPosition(int index)
    {
        //var random = new Random((uint)(114514 + index * 2 - 8848));
        //var dir = math.normalizesafe(random.NextFloat3() - new float3(0.5f, 0.5f, 0.5f));
        //var pos = dir * 50f;
        //var localToWorld = new LocalToWorld
        //{
        //    Value = float4x4.TRS(pos, quaternion.LookRotationSafe(dir, math.up()), new float3(1.0f, 1.0f, 1.0f))
        //};
        int uindex = math.abs(index);
        float3 pos = uindex % 2 == 0 ? new float3(10, 0, -10) : new float3(-10, 0, 10);
        var localToWorld = new LocalToWorld
        {
            Value = float4x4.TRS(pos, quaternion.identity, new float3(1.0f, 1.0f, 1.0f))
        };
        return localToWorld;
    }
}


public partial struct GenerateHostile : IJobEntity
{
    [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;
    public EntityCommandBuffer ECB;
    public double StartTime;

    public void Execute(GenerateEnemiesAspect aspect)
    {  
        var instance = ECB.Instantiate(aspect.HostilePrefab);
        ECB.SetComponent(instance, new LocalTransform
        {
            Position = aspect.Point1,
            Rotation = quaternion.identity,
            Scale = LocalTransformLookup[aspect.HostilePrefab].Scale
        });
        ECB.AddComponent(instance,new LiftCycleData 
        {
            LiftTime = 3.0f,
            StartTime = this.StartTime,
        });
    }
}