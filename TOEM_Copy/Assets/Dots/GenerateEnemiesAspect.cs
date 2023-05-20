using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public readonly partial struct GenerateEnemiesAspect : IAspect
{
    readonly RefRO<GenerateEnemiesData> m_Data;

    public Entity HostilePrefab => m_Data.ValueRO.HostilePrefab;
    public int HostileCount => m_Data.ValueRO.HostileCount;

    public float3 Point1 => new float3(10, 0, -10);
    public float3 Point2 => new float3(-10, 0, 10);
    public float3 Point3 => new float3(10, 0, 10);
    public float3 Point4 => new float3(-10, 0, -10);
}
