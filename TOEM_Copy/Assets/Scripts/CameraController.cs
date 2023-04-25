using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour, IController
{
    private Transform target;

    private Vector3 offset;

    private bool _inited;

    public void Init()
    {
        target = FindObjectOfType<TeamManager>().transform;
        offset = target.position - transform.position;
        _inited = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!_inited) { return; }
        transform.position = target.position - offset;
    }
}
