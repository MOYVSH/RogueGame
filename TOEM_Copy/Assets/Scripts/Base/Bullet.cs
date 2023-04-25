using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Hostile,
    Team
}

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public bool NeedDevastate = false;
    [HideInInspector]
    public BulletType _type;

    private Vector3 _dir;

    private bool _inited = false;
    private GameManager Contex;

    public void Init(GameManager contex, Vector3 dir, BulletType type)
    {
        this.Contex = contex;
        this._type = type;
        this._dir = dir;
        NeedDevastate = false;
        _inited = true;
    }

    public void update()
    {
        if (!_inited) return;
        if (NeedDevastate) return;
        transform.position += _dir * 0.3f;
    }
}
