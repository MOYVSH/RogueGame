namespace SangsTools
{
    public abstract class PoolMember<T> : IPoolMember where T : PoolMember<T> 
    {
        private static Pool_AutoCreate<T> pool;
        static PoolMember () 
        {
            pool = new Pool_AutoCreate<T> ();
        }

        public static T GetMember () => pool.GetMember ();
        public static void RecycleMember (T member) => pool.RecycleMember (member);
        public static void RecycleAll () => pool.RecycleAll ();

        public bool Active { get; private set; }

        protected PoolMember () {

        }

        public void RecycleSelf () {
            RecycleMember (this as T);
        }

        void IPoolMember.SetActive (bool active) {
            Active = active;
            OnSetActive (active);
        }

        protected abstract void OnSetActive (bool active);

    }
}