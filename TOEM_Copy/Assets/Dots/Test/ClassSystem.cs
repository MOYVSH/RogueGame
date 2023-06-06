using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class ClassSystem : SystemBase
{
    protected override void OnStartRunning()
    {
        base.OnStartRunning();
    }
    protected override void OnUpdate()
    {
        Entities.WithoutBurst().ForEach(() => { }).Run();
        foreach (var (transform, entity) in SystemAPI.Query<RefRW<LocalTransform>>()
                      .WithEntityAccess())
        {

        }
    }
}