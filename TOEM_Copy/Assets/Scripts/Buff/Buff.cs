using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    InvincibleModifier,
}

public enum BuffEffect
{
    Invincible,
}

public interface IBuff
{
    //还未加入到Buff容器中
    void OnBuffAwake();
    //加入到Buff容器后
    void OnBuffStart();
    //Buff添加时存在相同类型
    void OnBuffRefresh();
    //Buff销毁前 还未从Buff容器中移除
    void OnBuffRemove();
    //Buff销毁后 已从Buff容器中移除
    void OnBuffDestroy();
    //以创建定时器，以触发间隔持续效果
    void StartIntervalThink();
    void OnIntervalThink();
}

public class Buff: IBuff
{
    public BuffType BuffTypeId;
    public int OwnerID;
    public int CasterID;
    public int Level;
    public float Dution;
    public int Tag;
    public int ImmuneTag;
    public int Context;
    public long BeginTime;
    public long EndTime;
    public List<BuffEffect> Effects;

    public virtual void OnBuffAwake()
    { 
    
    }
    public virtual void OnBuffStart()
    { 
    
    }

    public virtual void OnBuffRefresh()
    {

    }

    public virtual void OnBuffRemove()
    {

    }
    public virtual void OnBuffDestroy()
    {

    }

    public virtual void StartIntervalThink()
    {

    }
    public virtual void OnIntervalThink()
    {

    }
}

public class InvincibleModifier : Buff
{
    public InvincibleModifier(int CasterID, int OwnerID)
    {
        this.OwnerID = OwnerID;
        this.CasterID = CasterID;
        BuffTypeId = BuffType.InvincibleModifier;
        Dution = 3f;
        BeginTime = TimeUtil.GetCurrentTimeStamp(false);
        EndTime = TimeUtil.ConvertDateTimeToUtc_13(DateTime.UtcNow.AddSeconds(Dution));
        Effects = new List<BuffEffect>();
        Effects.Add(BuffEffect.Invincible);
        OnBuffAwake();
    }

    public override void OnBuffAwake()
    {
        base.OnBuffAwake();
    }
}