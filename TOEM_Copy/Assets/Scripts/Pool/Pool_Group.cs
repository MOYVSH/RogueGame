using System;
using System.Collections.Generic;

namespace SangsTools {
    public class Pool_Group<BaseType> where BaseType : IPoolMember {
        private class ChildPool : Pool_AutoCreate<BaseType> {
            public readonly Type childType;
            public ChildPool (Type type) {
                childType = type;
            }
            protected override BaseType CreateMember () {
                return (BaseType) Activator.CreateInstance (childType);
            }
        }

        private readonly SortedList<Type, ChildPool> dictPool;
        private readonly Type baseType;

        public Pool_Group () {
            dictPool = new SortedList<Type, ChildPool> ();
            baseType = typeof (BaseType);
        }

        public T GetMember<T> () where T : class, BaseType {
            return GetMember (typeof (T)) as T;
        }
        public BaseType GetMember (Type type) {
            if (baseType.IsAssignableFrom (type)) {
                ChildPool pool;
                if (!dictPool.TryGetValue (type, out pool))
                    dictPool.Add (type, pool = new ChildPool (type));
                return pool.GetMember ();
            } else
                return default;
        }

        public void RecycleMember (BaseType member) {
            if (null == member)
                return;
            if (dictPool.TryGetValue (member.GetType (), out var pool))
                pool.RecycleMember (member);
        }
        public void RecycleMember<T> (T member) where T : class, BaseType {
            RecycleMember (member);
        }

        public void RecycleAll () {
            for (var i = 0; i < dictPool.Values.Count; i++) {
                dictPool.Values[i].RecycleAll ();
            }
        }

        public void RecycleAll<T> () where T : class, BaseType {
            if (dictPool.TryGetValue (typeof (T), out var pool))
                pool.RecycleAll ();
        }
    }

}