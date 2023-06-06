using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ColorAuthoring : MonoBehaviour
{
    public Material red;
    public Material green;
    public Material blue;
}

public class ColorAuthoringBaker : Baker<ColorAuthoring>
{
    public override void Bake(ColorAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent<ColorData>(entity);
    }
}
public class ColorData : IComponentData
{
    public Material red;
    public Material green;
    public Material blue;
}
