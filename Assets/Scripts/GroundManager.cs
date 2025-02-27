using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static float GroundLayerY;
    public static float GroundLayerInterval;

    public float m_GroundLayerY;
    public float m_GroundLayerInterval;

    private void Awake()
    {
        GroundLayerY = m_GroundLayerY;
        GroundLayerInterval = m_GroundLayerInterval;
    }
}
