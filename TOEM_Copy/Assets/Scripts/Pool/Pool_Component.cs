using UnityEngine;

namespace SangsTools
{
    public enum EM_SetActiveType
    {
        SetGameObjActive,
        SetScale,
    }

    public class Pool_Component<T> : PoolBase<T> where T : Component
    {

        public T prefab;
        public EM_SetActiveType setActiveType;

        public void Initlize(T prefab, EM_SetActiveType setActiveType = EM_SetActiveType.SetGameObjActive)
        {
            base.Initlize();
            this.prefab = prefab;
            this.setActiveType = setActiveType;
        }

        protected override T CreateMember () {
            return MonoBehaviour.Instantiate (prefab);
        }

        protected override void SetMemberActive (T member, bool active) {
            switch (setActiveType) {
                case EM_SetActiveType.SetGameObjActive:
                    member.gameObject.SetActive (active);
                    break;
                case EM_SetActiveType.SetScale:
                    member.transform.localScale = active?prefab.transform.localScale : Vector3.zero;
                    break;
                default:
                    break;
            }

        }
    }
}