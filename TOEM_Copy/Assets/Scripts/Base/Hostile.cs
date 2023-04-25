using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostile : MonoBehaviour
{
    [HideInInspector]
    public bool NeedDevastate = false;

    HostileModel data;
    private bool _inited = false;
    private GameManager Contex;

    public void Init(GameManager contex,int ID)
    {
        this.Contex = contex;
        data = new HostileModel();
        data.Init(ID);
        NeedDevastate = false;
        _inited = true;
    }

    public void update()
    {
        if (!_inited)  return;
        if (NeedDevastate) return;
        if (Vector3.Distance(transform.position, GameManager.Instance.TeamManager.transform.position) < 0.2f)
        {
            //Vector3 dir = Vector3.Normalize(GameManager.Instance.TeamManager.transform.position - transform.position);
            //transform.position += dir * Time.deltaTime * data.BaseSpeed;
            //NeedDevastate = true;
            return;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, GameManager.Instance.TeamManager.transform.position, Time.deltaTime * data.BaseSpeed);
        }
    }
}
