using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileModel : IModel
{
    public int ID;

    private float _baseSpeed;

    public float BaseSpeed
    {
        get 
        { 
            return _baseSpeed; 
        }
        set
        {
            if (_baseSpeed != value)
            {
                _baseSpeed = value;
            }
        }
    }

    public void Init(int ID)
    {
        this.ID = ID;
        BaseSpeed = 1f;
    }
}
