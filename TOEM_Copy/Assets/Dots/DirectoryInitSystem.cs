using System;
using Unity.Entities;
using UnityEngine;

public partial struct DirectoryInitSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        // We need to wait for the scene to load before Updating, so we must RequireForUpdate at
        // least one component type loaded from the scene.
    }

    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        var go = GameObject.Find("GameManager");
        if (go == null)
        {
            throw new Exception("GameObject 'Directory' not found.");
        }

        var gameManager = go.GetComponent<GameManager>();

        var directoryManaged = new DirectoryManaged();
        directoryManaged._GameManager = gameManager;

        var entity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponentData(entity, directoryManaged);
    }
}

public class DirectoryManaged : IComponentData
{
    public GameManager _GameManager;

    // Every IComponentData class must have a no-arg constructor.
    public DirectoryManaged()
    {
    }
}