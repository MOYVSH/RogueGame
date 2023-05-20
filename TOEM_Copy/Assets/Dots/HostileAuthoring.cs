using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

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

public class HostileAuthoring : MonoBehaviour
{
}