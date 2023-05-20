using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct FacingCameraSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithEntityAccess()
                     .WithAll<FacingCameraData>())
        {
            transform.ValueRW.Rotation = CameraSingleton.Instance.transform.rotation;
        }
    }
}
