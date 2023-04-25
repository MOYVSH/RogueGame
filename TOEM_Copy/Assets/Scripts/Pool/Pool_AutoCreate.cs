using System;

namespace SangsTools 
{
    public class Pool_AutoCreate<T> : PoolBase<T> where T : IPoolMember
    {
        protected override T CreateMember ()
        {
            return Activator.CreateInstance<T> ();
        }

        protected override void SetMemberActive (T member, bool active)
        {
            member.SetActive (active);
        }
    }
}