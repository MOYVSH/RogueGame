using Unity.Entities;
using Unity.Mathematics;


public readonly partial struct GenerateEnemiesAspect : IAspect
{
    readonly RefRO<GenerateEnemiesData> m_Data;

    public Entity HostilePrefab => m_Data.ValueRO.HostilePrefab;

    public float3 Point1 => new float3(10, 0, -10);
    public float3 Point2 => new float3(-10, 0, 10);
    public float3 Point3 => new float3(10, 0, 10);
    public float3 Point4 => new float3(-10, 0, -10);
}
