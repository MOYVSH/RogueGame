using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleModel : IModel
{
    public string Name;

    public void Init()
    {
        Name = string.Empty;
    }

    public void Init(int ID)
    {
        Name = string.Empty;
    }
}
