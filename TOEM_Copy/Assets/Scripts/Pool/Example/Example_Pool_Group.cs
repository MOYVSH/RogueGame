using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SangsTools.Example {
    /*
        用来处理一个基类，多个继承类的回收跟创建
    */

    public interface IRole : IPoolMember {
        void DoSomeThing ();
    }
    public class Role_Player : IRole {
        public void DoSomeThing () {
            //Player do someting
        }

        public void SetActive (bool active) { }
    }
    public class Role_Enemy : IRole {
        public void DoSomeThing () {
            //Player do someting
            Debug.Log ("Enemy do someting");
        }
        public void SetActive (bool active) { }
    }
    public class Role_Boss : IRole {
        public void DoSomeThing () {
            //Player do someting
            Debug.Log ("Boss do someting");
        }
        public void SetActive (bool active) { }
    }

    public class Example_Pool_Group {
        public void Test () {
            var poolGroup = new Pool_Group<IRole> ();

            IRole player = poolGroup.GetMember<Role_Player> ();

            IRole[] arrEnemy = new IRole[3];
            for (var i = 0; i < arrEnemy.Length; i++) {
                arrEnemy[i] = poolGroup.GetMember<Role_Enemy> ();
            }

            IRole boss = poolGroup.GetMember (typeof (Role_Boss));

            player.DoSomeThing ();
            for (var i = 0; i < arrEnemy.Length; i++) {
                arrEnemy[i].DoSomeThing ();
            }
            boss.DoSomeThing ();

            poolGroup.RecycleAll ();
        }
    }
}