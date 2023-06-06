using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

// This system should run after the transform system has been updated, otherwise the camera
// will lag one frame behind the tank.
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateAfter(typeof(LiftCycleSystem))]
[BurstCompile]
public partial struct CameraSystem : ISystem
{
    Entity target;
    float3 offset;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        offset = new float3(0, 10, -8);
    }

    // Because this OnUpdate accesses managed objects, it cannot be Burst compiled.
    public void OnUpdate(ref SystemState state)
    {
        var HostileQuery = SystemAPI.QueryBuilder().WithAll<HostileData>().Build();
        var Hostiles = HostileQuery.ToEntityArray(Allocator.Temp);
        if (Hostiles.Length == 0)
        {
            return;
        }

        target = Hostiles[0];

        var targetTransform = SystemAPI.GetComponent<LocalToWorld>(target);
        var cameraTransform = CameraSingleton.Instance.transform;

        cameraTransform.position = targetTransform.Position + offset;
    }
}
