using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHostile : Bullet
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Team")
        {
            NeedDevastate = true;
        }
    }
}
