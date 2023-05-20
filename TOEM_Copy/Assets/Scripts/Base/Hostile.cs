using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostile : MonoBehaviour
{
    [SerializeField]
    private GroupController myGroup;//��ǰ��Ա��GroupController���

    //�ٶȺ��ƶ���ز���
    private float targetSpeed;
    private float speed;
    private float currentSpeed;
    private Vector3 myMovement;

    [Header("���ڵ���ID")]
    public int groupId;
    [Header("�ƶ��ٶ�")]
    public float moveSpeed;
    [Header("��ת�ٶ�")]
    public float rotateSpeed;

    Hostile otherMember;

    [HideInInspector]
    public bool NeedDevastate = false;

    HostileModel data;
    private bool _inited = false;
    private GameManager Contex;

    public void Init(GameManager contex,int ID)
    {
        this.Contex = contex;
        data = new HostileModel();
        data.Init(ID);
        NeedDevastate = false;
        _inited = true;

        moveSpeed = data.BaseSpeed;
        myGroup = GroupController.GetGroup(groupId);
    }

    public void update()
    {
        if (!_inited)  return;
        if (NeedDevastate) return;


        Vector3 dis = myGroup.transform.position - transform.position;
        Vector3 dir = dis.normalized;

        //���¼���Ŀ�ĵؾ���Ȩ��
        if (dis.magnitude < myGroup.targetCloseDistance)
        {
            dir *= dis.magnitude / myGroup.targetCloseDistance;
        }
        dir += GetAroundMemberInfo();//��ȡ��Χ����ƶ�

        //�����ƶ��ٶ�
        if ((myGroup.transform.position - transform.position).magnitude < myGroup.stopDis)
        {
            targetSpeed = 0;
        }
        else
        {
            targetSpeed = moveSpeed;
        }
        speed = Mathf.Lerp(speed, targetSpeed, 2 * Time.deltaTime);

        //�����������������������������������������ƶ�
        transform.right = -dir;
        Move(dir, speed);
    }



     /// <summary>
    /// �õ���Χ��Ա����Ϣ
    /// </summary>
    /// <returns></returns>
    private Vector3 GetAroundMemberInfo()
    {
        Collider[] c = Physics.OverlapSphere(transform.position, myGroup.keepDis, myGroup.mask.value);//��ȡ��Χ��Ա
        Vector3 dis;
        Vector3 v1 = Vector3.zero;
        Vector3 v2 = Vector3.zero;
        for (int i = 0; i < c.Length; i++)
        {
            otherMember = c[i].GetComponent<Hostile>();
            dis = transform.position - otherMember.transform.position;//����
            v1 += dis.normalized * (1 - dis.magnitude / myGroup.keepDis);//�鿴����Χ��Ա�ľ���
            v2 += otherMember.myMovement;//�鿴��Χ��Ա�ƶ�����

        }
        return v1.normalized * myGroup.keepWeight + v2.normalized;//���Ȩ������
    }

    /// <summary>
    /// �ƶ�
    /// </summary>
    /// <param name="_dir">����</param>
    /// <param name="_speed">�ٶ�</param>
    private void Move(Vector3 _dir, float _speed)
    {
        Vector3 finialDirection = _dir.normalized;
        float finialSpeed = _speed;
        float forwardDir = Vector3.Dot(finialDirection, transform.forward);


        if (forwardDir < -0.2f)
        {
            finialSpeed = Mathf.Lerp(currentSpeed, -_speed * 8, 4 * Time.deltaTime);
        }

        finialSpeed *= Mathf.Clamp01(_dir.magnitude);

        transform.Translate(Vector3.left * finialSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        currentSpeed = finialSpeed;
        myMovement = _dir * finialSpeed;
    }
}
