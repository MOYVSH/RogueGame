using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModel : IModel
{
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
        BaseSpeed = 1f;
    }
}
