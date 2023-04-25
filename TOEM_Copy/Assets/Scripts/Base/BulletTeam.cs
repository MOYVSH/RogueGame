using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTeam : Bullet
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hostile")
        {
            NeedDevastate = true;
        }
    }
}
