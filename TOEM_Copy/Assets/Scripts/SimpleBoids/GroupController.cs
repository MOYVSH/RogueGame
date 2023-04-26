using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ����ܿ����������ص��鳤���ϣ����������Ҳ������Ҫ��������壩
/// </summary>
public class GroupController : MonoBehaviour
{
    private static List<GroupController> groups;//������

    [Header("���г�Ա�Ĳ�")]
    public LayerMask mask;
    [Header("���г�Ա��ID")]
    public int groupID = 0;
    [Header("���г�Աʼ�ձ��ֵľ���")]
    public float keepDis;
    [Header("���г�Աʼ�ձ��ֵľ����Ȩ��")]
    public float keepWeight;
    [Header("���پ��������̫��")]
    public float targetCloseDistance;
    [Header("���г�Աֹͣ�ƶ��ľ���")]
    public float stopDis;

    [Header("��ʾ��Χ��")]
    public bool ShowDebug;
    /// <summary>
    /// �õ���Ա�����ĸ���
    /// </summary>
    /// <param name="index">��ԱID</param>
    /// <returns></returns>
    public static GroupController GetGroup(int index)
    {
        if (groups == null)
        {
            groups = new List<GroupController>(FindObjectsOfType(typeof(GroupController)) as GroupController[]);
        }

        for (int i = 0; i < groups.Count; i++)
        {
            if (groups[i].groupID == index)
            {
                return groups[i];
            }
        }
        throw new System.Exception("û���ҵ���ͬID����");
    }
}