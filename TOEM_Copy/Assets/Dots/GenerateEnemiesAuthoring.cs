using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GenerateEnemiesAuthoring : MonoBehaviour
{
    public GameObject HostilePrefab;
    public int HostileCount;
}

public class GenerateEnemiesBaker : Baker<GenerateEnemiesAuthoring>
{
    public override void Bake(GenerateEnemiesAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new GenerateEnemiesData
        {
            HostilePrefab = GetEntity(authoring.HostilePrefab, TransformUsageFlags.Dynamic),
            HostileCount = authoring.HostileCount
        });
    }
}

public struct GenerateEnemiesData : IComponentData
{
    public Entity HostilePrefab;
    public int HostileCount;
}