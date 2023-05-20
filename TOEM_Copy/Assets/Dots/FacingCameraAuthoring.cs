using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class FacingCameraBaker : Baker<FacingCameraAuthoring>
{
    public override void Bake(FacingCameraAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent<FacingCameraData>(entity);
    }
}

public struct FacingCameraData : IComponentData
{

}

public class FacingCameraAuthoring : MonoBehaviour
{

}

