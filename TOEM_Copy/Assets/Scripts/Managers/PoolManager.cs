using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour, IManager
{
    private Dictionary<string, Pool_GameObject> PoolDic = new Dictionary<string, Pool_GameObject>();

    public void Init() 
    {
        PoolDic.Clear();
        foreach (var item in GetComponentsInChildren<Pool_GameObject>())
        {
            PoolDic.Add(item.Type, item);
        }
    }

    public Pool_GameObject GetPool(string Type)
    {
        return PoolDic.TryGetValue(Type, out Pool_GameObject pool) ? pool : null;
    }
}
