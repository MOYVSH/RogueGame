using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private RoleModel data;
    private TeamManager _teamManager;
    public void Init(TeamManager teamManager)
    {
        _teamManager = teamManager;
        data = new RoleModel();
        data.Init();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hostile")
        {
            if (_teamManager._gameManager.BuffManager.ContainBuff(BuffType.InvincibleModifier, _teamManager.data.ID))
            {
                //Debug.LogError(1);
                return;
            }
            else
            {
                //Debug.LogError(2);

                _teamManager._gameManager.BuffManager.AddBuff(BuffType.InvincibleModifier, _teamManager.data.ID, _teamManager.data.ID);
            }
        }
    }
}
