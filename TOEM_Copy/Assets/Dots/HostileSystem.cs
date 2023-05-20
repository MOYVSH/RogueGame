using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[BurstCompile]
public partial struct HostileSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        //state.RequireForUpdate<HostileMovement>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;

        foreach (var (transform, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithEntityAccess()
                     .WithAll<HostileData>())
        {
            var pos = transform.ValueRO.Position;

            // This does not modify the actual position of the tank, only the point at
            // which we sample the 3D noise function. This way, every tank is using a
            // different slice and will move along its own different random flow field.
            pos.y = entity.Index;
            var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;

            var dir = float3.zero;
            math.sincos(angle, out dir.x, out dir.z);
            transform.ValueRW.Position += dir * dt * 5.0f;
            //transform.ValueRW.Rotation = quaternion.RotateY(angle);
        }
    }
}
