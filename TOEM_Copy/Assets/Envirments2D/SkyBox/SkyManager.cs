using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SkyManager : MonoBehaviour
{
    public Transform m_Sun;
    public Color m_SunColor;
    // Update is called once per frame
    void Update()
    {
        if (m_Sun != null)
        {
            Matrix4x4 LtoW = m_Sun.localToWorldMatrix;
            RenderSettings.skybox.SetMatrix("LtoW", LtoW);
        }
        RenderSettings.skybox.SetColor("_SunColor", m_SunColor);
    }
}
