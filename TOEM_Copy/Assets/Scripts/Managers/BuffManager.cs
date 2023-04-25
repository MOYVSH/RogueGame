using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;

public class BuffManager:IManager
{
    List<Buff> _buffs;

    public BuffManager()
    {
        _buffs = new List<Buff>();
    }

    public void Init()
    {
        var clock1 = UnityTimerMgr.CreateFrame2Clock((v) =>
        {
            for (int i = 0; i < _buffs.Count; i++)
            {
                if (_buffs[i].EndTime <= TimeUtil.GetCurrentTimeStamp(false))
                {
                    RemoveBuff(_buffs[i]);
                }
            }
        });
        clock1.Start();
    }

    public bool ContainBuff(BuffType buffType,int ownerID)
    {
        for (int i = 0; i < _buffs.Count; i++)
        {
            if (_buffs[i].BuffTypeId == buffType && _buffs[i].OwnerID == ownerID)
                return true;
        }
        return false;
    }

    public void AddBuff(BuffType buffType,int casterID,int ownerID)
    {
        Buff buff = null;
        switch (buffType)
        {
            case BuffType.InvincibleModifier:
                buff = new InvincibleModifier(casterID, ownerID);
                break;
            default:
                return;
        }
        //Debug.LogError("AddBuff");
        _buffs.Add(buff);
    }

    public void RemoveBuff(Buff buff) 
    {
        //Debug.LogError("RemoveBuff");
        _buffs.Remove(buff);
    }
}
