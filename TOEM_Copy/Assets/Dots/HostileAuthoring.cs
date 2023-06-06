using HMH.ECS.SpatialHashing;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public class HostileAuthoring : MonoBehaviour
{

}

public class HostileAuthoringBaker : Baker<HostileAuthoring>
{
    public override void Bake(HostileAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent<HostileData>(entity);
    }
}
public struct HostileData : IComponentData
{

}
