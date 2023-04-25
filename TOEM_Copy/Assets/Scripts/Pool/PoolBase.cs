using System;
using System.Collections.Generic;
using UnityEngine;

namespace SangsTools 
{
    public abstract class PoolBase<T> : MonoBehaviour,IPool<T> 
    {
        [SerializeField]
        private List<T> activeMember;
        private Stack<T> unActiveMember;

        private int[] _count;
        public int[] Count 
        {
            get 
            {
                _count[0] = activeMember.Count;
                _count[1] = unActiveMember.Count;
                return _count;
            }
        }

        public virtual void Initlize()
        {
            _count = new int[2];
            activeMember = new List<T>();
            unActiveMember = new Stack<T>();
        }

        public T GetMember () 
        {
            T member;
            if (unActiveMember.Count > 0)
                member = unActiveMember.Pop ();
            else
                member = CreateMember ();

            SetMemberActive (member, true);
            return member;
        }

        public void RecycleMember (T member) 
        {
            if (null == member)
                return;
            if (activeMember.Contains (member) && !unActiveMember.Contains (member)) 
            {
                SetMemberActive (member, false);
                activeMember.Remove (member);
                unActiveMember.Push (member);
            }
        }

        public void RecycleAll () 
        {
            for (var i = 0; i < activeMember.Count; i++)
            {
                SetMemberActive (activeMember[i], false);
                unActiveMember.Push (activeMember[i]);
            }
            activeMember.Clear ();
        }

        protected abstract T CreateMember ();
        protected abstract void SetMemberActive (T member, bool active);
    }

}