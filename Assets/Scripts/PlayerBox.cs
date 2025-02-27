using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBox : PlayerTowerUnit
{
    public Slider m_Slider;

    private void UpdateSlider()
    {
        m_Slider.value = (float) _currentHitPoint / m_HitPoint;
    }

    private void Awake()
    {
        _playerTower = GetComponentInParent<PlayerTower>();
        CurrentHitPoint = m_HitPoint;
        HitPointManager.CachedHitPoint.Add(m_Collider2D, this);
        UpdateSlider();
    }

    private void OnEnable()
    {
        OnCurrentHitPointUpdated += UpdateSlider;
        OnDeath += DestroyTower;
    }

    private void OnDisable()
    {
        OnCurrentHitPointUpdated -= UpdateSlider;
        OnDeath -= DestroyTower;
    }

    private void DestroyTower()
    {
        _playerTower.RemoveTowerUnit(this);
    }
}
