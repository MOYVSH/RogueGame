using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载到组中的每个成员身上
/// </summary>
public class GroupMember : MonoBehaviour
{
    [SerializeField]
    private GroupController myGroup;//当前成员的GroupController组件

    //速度和移动相关参数
    private float targetSpeed;
    private float speed;
    private float currentSpeed;
    private Vector3 myMovement;

    [Header("属于的组ID")]
    public int groupId;
    [Header("移动速度")]
    public float moveSpeed;
    [Header("旋转速度")]
    public float rotateSpeed;

    GroupMember otherMember;

    private void Start()
    {
        myGroup = GroupController.GetGroup(groupId);
    }

    void Update()
    {
        Vector3 dis = myGroup.transform.position - transform.position;
        Vector3 dir = dis.normalized;

        //重新计算目的地距离权重
        if (dis.magnitude < myGroup.targetCloseDistance)
        {
            dir *= dis.magnitude / myGroup.targetCloseDistance;
        }
        dir += GetAroundMemberInfo();//获取周围组的移动

        //计算移动速度
        if ((myGroup.transform.position - transform.position).magnitude < myGroup.stopDis)
        {
            targetSpeed = 0;
        }
        else
        {
            targetSpeed = moveSpeed;
        }
        speed = Mathf.Lerp(speed, targetSpeed, 2 * Time.deltaTime);

        //————————————————————移动
        transform.right = -dir;
        Move(dir, speed);
    }

    private void OnDrawGizmos()
    {        
        //绘制球形线框范围 和单位之间的连线
        Gizmos.color = Color.red;
        if (myGroup && myGroup.ShowDebug)
        {
            if (myGroup)
            {
                Gizmos.DrawWireSphere(transform.position, myGroup.keepDis);
            }
            if (otherMember) 
            {
                Debug.DrawLine(transform.position, otherMember.transform.position, Color.yellow);
            }
        }

    }

    /// <summary>
    /// 得到周围成员的信息
    /// </summary>
    /// <returns></returns>
    private Vector3 GetAroundMemberInfo()
    {
        Collider[] c = Physics.OverlapSphere(transform.position, myGroup.keepDis, myGroup.mask.value);//获取周围成员
        Vector3 dis;
        Vector3 v1 = Vector3.zero;
        Vector3 v2 = Vector3.zero;
        for (int i = 0; i < c.Length; i++)
        {
            otherMember = c[i].GetComponent<GroupMember>();
            dis = transform.position - otherMember.transform.position;//距离
            v1 += dis.normalized * (1 - dis.magnitude / myGroup.keepDis);//查看与周围成员的距离
            v2 += otherMember.myMovement;//查看周围成员移动方向

        }
        return v1.normalized * myGroup.keepWeight + v2.normalized;//添加权重因素
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="_dir">方向</param>
    /// <param name="_speed">速度</param>
    private void Move(Vector3 _dir, float _speed)
    {
        Vector3 finialDirection = _dir.normalized;
        float finialSpeed = _speed, finialRotate = 0;
        float rotateDir = Vector3.Dot(finialDirection, transform.right);
        float forwardDir = Vector3.Dot(finialDirection, transform.forward);

        if (forwardDir < 0)
        {
            rotateDir = Mathf.Sign(rotateDir);
        }
        if (forwardDir < -0.2f)
        {
            finialSpeed = Mathf.Lerp(currentSpeed, -_speed * 8, 4 * Time.deltaTime);
        }

        //——————————防抖
        if (forwardDir < 0.98f)
        {
            finialRotate = Mathf.Clamp(rotateDir * 180, -rotateSpeed, rotateSpeed);
        }

        finialSpeed *= Mathf.Clamp01(_dir.magnitude);
        finialSpeed *= Mathf.Clamp01(1 - Mathf.Abs(rotateDir) * 0.8f);

        transform.Translate(Vector3.left * finialSpeed * Time.deltaTime);
        //transform.Rotate(Vector3.forward * finialRotate * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0,0);

        currentSpeed = finialSpeed;
        myMovement = _dir * finialSpeed;
    }
}