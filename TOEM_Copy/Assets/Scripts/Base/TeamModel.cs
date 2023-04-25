using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamModel : IModel
{
    public int ID;

    private int _maxSlotCount;
    public int MaxSlotCount
    {
        get
        {
            return _maxSlotCount;
        }
    }

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
        _maxSlotCount = 1;
        BaseSpeed = 5f;
    }
}
