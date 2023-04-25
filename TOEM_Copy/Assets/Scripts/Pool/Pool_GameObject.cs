using System.Collections.Generic;
using UnityEngine;

public enum SetActiveType
{
    SetGameObjActive,
    SetScale,
}

public class Pool_GameObject : MonoBehaviour
{
    public string Type;
    public int BaseCount;
    public GameObject prefab;
    public SetActiveType setActiveType;


    [SerializeField]
    private List<GameObject> activeMember;
    private Stack<GameObject> unActiveMember;

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


    //public override void Awake()
    //{
    //    base.Awake();
    //    Initlize(BaseCount, setActiveType);
    //}

    public void Initlize(int Count, SetActiveType setActiveType = SetActiveType.SetGameObjActive)
    {
        _count = new int[2];
        activeMember = new List<GameObject>();
        unActiveMember = new Stack<GameObject>();
        for (int i = 0; i < Count; i++)
        {
            var go = CreateMember();
            go.transform.SetParent(this.transform, false);
            go.SetActive(false);
            unActiveMember.Push(go);
        }
    }

    public GameObject GetMember()
    {
        GameObject member;
        if (unActiveMember.Count > 0)
            member = unActiveMember.Pop();
        else
            member = CreateMember();

        SetMemberActive(member, true);
        activeMember.Add(member);
        return member;
    }

    public void RecycleMember(GameObject member)
    {
        if (null == member)
            return;

        if (activeMember.Contains(member) && !unActiveMember.Contains(member))
        {
            SetMemberActive(member, false);
            activeMember.Remove(member);
            unActiveMember.Push(member);
            member.transform.SetParent(transform, false);
        }
    }

    public void RecycleAll()
    {
        for (var i = 0; i < activeMember.Count; i++)
        {
            SetMemberActive(activeMember[i], false);
            unActiveMember.Push(activeMember[i]);
        }
        activeMember.Clear();
    }

    private GameObject CreateMember()
    {
        return Instantiate(prefab);
    }

    private void SetMemberActive(GameObject member, bool active)
    {
        switch (setActiveType)
        {
            case SetActiveType.SetGameObjActive:
                member.gameObject.SetActive(active);
                break;
            case SetActiveType.SetScale:
                member.transform.localScale = active ? prefab.transform.localScale : Vector3.zero;
                break;
            default:
                break;
        }

    }
}
