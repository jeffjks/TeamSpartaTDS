using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : HasHitPoint
{
    public Collider2D m_Collider2D;
    public Slider m_Slider;

    protected override void OnCurrentHitPointUpdated()
    {
        m_Slider.value = (float) _currentHitPoint / m_HitPoint;
    }

    private void Start()
    {
        CurrentHitPoint = m_HitPoint;
        HitPointManager.CachedHitPoint.Add(m_Collider2D, this);
    }
}
