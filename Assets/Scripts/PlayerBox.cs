using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBox : HasHitPoint
{
    public Collider2D m_Collider2D;
    public Slider m_Slider;

    private void UpdateSlider()
    {
        m_Slider.value = (float) _currentHitPoint / m_HitPoint;
    }

    private void Awake()
    {
        CurrentHitPoint = m_HitPoint;
        HitPointManager.CachedHitPoint.Add(m_Collider2D, this);
    }

    private void OnEnable()
    {
        m_UA_OnCurrentHitPointUpdated += UpdateSlider;
    }

    private void OnDisable()
    {
        m_UA_OnCurrentHitPointUpdated -= UpdateSlider;
    }
}
