using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour, IManager
{
    public Transform[] Slots = new Transform[5];


    [HideInInspector]
    public TeamModel data;
    [HideInInspector]
    public GameManager _gameManager;
    [HideInInspector]
    public MoveController _moveController;

    public void SetGameManager(GameManager manager)
    {
        _gameManager = manager;
    }

    public void Init(int ID)
    {
        data = new TeamModel();
        data.Init(ID);

        _moveController = GetComponent<MoveController>();
        _moveController.Init();
        _moveController.enabled = true;

        for (int i = 0; i < Slots.Length; i++)
        {
            if (i > data.MaxSlotCount - 1) break;
            SetSlot(i);
        }
    }

    public void SetSlot(int index)
    {
        var go = _gameManager.PoolManager.GetPool("Ghost").GetMember();
        go.transform.SetParent(Slots[index]);
        go.transform.localPosition = Vector3.zero;
        go.AddComponent<Slot>().Init(this);
    }


}
