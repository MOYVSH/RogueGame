using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SangsTools.Example {
    /*
        构造函数私有，保证外部无法new
        好处是不需要外部用个变量去存对象池，内部封了一个
    */

    public class Bullet : PoolMember<Bullet> {
        private Bullet () {

        }
        public void DoSomeThing () {
            //DoSomeThing
        }
        protected override void OnSetActive (bool active) { }
    }

    public class Example_PoolMember 
    {
        public void Test () 
        {
            List<Bullet> list = new List<Bullet> ();

            for (var i = 0; i < 10; i++) 
            {
                //“Bullet.Bullet()”不可访问，因为它具有一定的保护级别
                // list.Add (new Bullet ());

                list.Add (Bullet.GetMember ());
            }

            for (var i = 0; i < list.Count; i++)
            {
                list[i].DoSomeThing ();
            }

            for (var i = 0; i < 5; i++) 
            {
                list[i].RecycleSelf ();
            }
            list.RemoveRange (0, 5);
            for (var i = 0; i < 5; i++) 
            {
                Bullet.RecycleMember (list[i]);
            }
            list.Clear ();

            for (var i = 0; i < 10; i++) 
            {
                list.Add (Bullet.GetMember ());
            }

            Bullet.RecycleAll ();
        }
    }
}