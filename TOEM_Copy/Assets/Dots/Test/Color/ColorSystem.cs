using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

[RequireMatchingQueriesForUpdate]
//[BurstCompile]
public partial class ColorSystem : SystemBase
{
    private Dictionary<Material, BatchMaterialID> m_MaterialMapping;

    protected override void OnStartRunning()
    {
        RequireForUpdate<ColorData>();
        var egs = World.GetOrCreateSystemManaged<EntitiesGraphicsSystem>();
        m_MaterialMapping = new Dictionary<Material, BatchMaterialID>();

        Entities.WithoutBurst().ForEach((ColorData data) => 
            {
                if (!m_MaterialMapping.ContainsKey(data.red))
                    m_MaterialMapping[data.red] = egs.RegisterMaterial(data.red);
                if (!m_MaterialMapping.ContainsKey(data.green))
                    m_MaterialMapping[data.green] = egs.RegisterMaterial(data.green);
                if (!m_MaterialMapping.ContainsKey(data.blue))
                    m_MaterialMapping[data.blue] = egs.RegisterMaterial(data.blue);
            }).Run();
    }

    protected override void OnDestroy()
    {
        m_MaterialMapping?.Clear();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        Entities
           .WithoutBurst()
           .ForEach((ColorData data, ref MaterialMeshInfo info, in LocalToWorld transform) =>
           {
               if (transform.Position.x * transform.Position.z >= 0)
               {
                   info.MaterialID = m_MaterialMapping[data.red];
               }
               else
               {
                   info.MaterialID = m_MaterialMapping[data.green];
               }
           }).Run();
    }
}
