using SangsTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>, IManager
{
    public GameObject TeamPrefab;

    //Managers
    private TeamManager _teamManager;
    public TeamManager TeamManager
    {
        get
        {
            if (_teamManager == null)
            {
                _teamManager = (TeamManager)FindObjectOfType(typeof(TeamManager));

            }
            return _teamManager;
        }
    }

    private PoolManager _poolManager;
    public PoolManager PoolManager
    {
        get
        {
            if (_poolManager == null)
            {
                _poolManager = (PoolManager)FindObjectOfType(typeof(PoolManager));

            }
            return _poolManager;
        }
    }

    private BuffManager _buffManager;
    public BuffManager BuffManager
    {
        get
        {
            if (_buffManager == null)
            {
                _buffManager = new BuffManager();
            }
            return _buffManager;
        }
    }

    //Controllers
    private CameraController _cameraController;
    public CameraController CameraController
    {
        get
        {
            if (_cameraController == null)
            {
                _cameraController = (CameraController)FindObjectOfType(typeof(CameraController));
            }
            return _cameraController;
        }
    }

    public List<Hostile> Hostiles => _hostiles;

    List<Hostile> _hostiles = new List<Hostile>();
    List<Bullet> _bullets = new List<Bullet>();

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        // 对象池初始化
        PoolManager.Init();
        PoolManager.GetPool("Ghost").Initlize(5);
        PoolManager.GetPool("GhostHostile").Initlize(6);
        PoolManager.GetPool("BulletHostile").Initlize(20);
        PoolManager.GetPool("BulletTeam").Initlize(20);

        GameObject.Instantiate(TeamPrefab, Vector3.zero, Quaternion.identity).GetComponent<TeamManager>();

        // 初始化场景的时候加载出来角色队伍
        TeamManager.SetGameManager(this);
        TeamManager.Init(1);

        // 在初始化角色之后初始化相机
        CameraController.Init();

        BuffManager.Init();


        //for (int i = 0; i < 20; i++)
        //{
        //    GenerateHostile(new Vector3(UnityEngine.Random.Range(-15, 15), 0, UnityEngine.Random.Range(-15, 15)), 2 + i);
        //}
        //GenerateHostile(new Vector3(-15, 0, 0), 2);
        //GenerateHostile(new Vector3(15, 0, 0),3);
        //GenerateHostile(new Vector3(0, 0, -15),4); 
        //GenerateHostile(new Vector3(-15, 0, -15),5);

        //BuffManager.AddBuff(BuffType.InvincibleModifier, 1, 1);
        //BuffManager.AddBuff(BuffType.InvincibleModifier, 2, 2);
        //BuffManager.AddBuff(BuffType.InvincibleModifier, 3, 3);
        //BuffManager.AddBuff(BuffType.InvincibleModifier, 4, 4);
        //BuffManager.AddBuff(BuffType.InvincibleModifier, 5, 5);

        //Job.MakeImmediately(cb(), this);
    }

    IEnumerator cb()
    {
        while (true) 
        {
            yield return Yielders.Get(0.2f);
            GenerateBullet(new Vector3(TeamManager.transform.position.x, 1, TeamManager.transform.position.z),
                            new Vector3(_hostiles[0].transform.position.x, 1, _hostiles[0].transform.position.z),
                            -1, BulletType.Team);
        }
    }

    public void GenerateHostile(Vector3 position,int ID)
    {
        var go = PoolManager.GetPool("GhostHostile").GetMember();
        go.transform.SetParent(null);
        go.transform.position = position;
        var hostile = go.GetComponent<Hostile>();
        hostile.Init(this, ID);
        _hostiles.Add(hostile);
    }

    public void DevastateHostile(Hostile hostile)
    {
        _hostiles.Remove(hostile);
        PoolManager.GetPool("GhostHostile").RecycleMember(hostile.gameObject);
    }

    public void GenerateBullet(Vector3 position, Vector3 target, int ID, BulletType type)
    {
        GameObject go = null;
        Bullet bullet = null;
        switch (type)
        {
            case BulletType.Hostile:
                go = PoolManager.GetPool("BulletHostile").GetMember();
                go.transform.SetParent(null);
                go.transform.position = position;
                bullet = go.GetComponent<BulletHostile>();
                bullet.Init(this, Vector3.Normalize(target - position), type);
                break;
            case BulletType.Team:
                go = PoolManager.GetPool("BulletTeam").GetMember();
                go.transform.SetParent(null);
                go.transform.position = position;
                bullet = go.GetComponent<BulletTeam>();
                bullet.Init(this, Vector3.Normalize(target - position), type);
                break;
            default:
                break;
        }
        _bullets.Add(bullet);
    }

    public void DevastateBullet(Bullet bullet)
    {
        _bullets.Remove(bullet);
        switch (bullet._type)
        {
            case BulletType.Hostile:
                PoolManager.GetPool("BulletHostile").RecycleMember(bullet.gameObject);
                break;
            case BulletType.Team:
                PoolManager.GetPool("BulletTeam").RecycleMember(bullet.gameObject);
                break;
            default:
                break;
        }
    }

    // 当前场景的GameLoop
    public void FixedUpdate()
    {
        for (int i = 0; i < _hostiles.Count; i++)
        {
            _hostiles[i].update();
            if (_hostiles[i].NeedDevastate)
            {
                DevastateHostile(_hostiles[i]);
            }
        }

        for (int i = 0; i < _bullets.Count; i++)
        {
            _bullets[i].update();
            if (_bullets[i].NeedDevastate)
            {
                DevastateBullet(_bullets[i]);
            }
        }
    }
}
