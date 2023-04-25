using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SangsTools {

    public interface IPool<T> {
        T GetMember ();
        void RecycleMember (T member);
        void RecycleAll ();
    }

}