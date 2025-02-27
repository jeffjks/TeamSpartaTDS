using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHero : PlayerTowerUnit
{
    private void Awake()
    {
        _playerTower = GetComponentInParent<PlayerTower>();
        CurrentHitPoint = m_HitPoint;
        HitPointManager.CachedHitPoint.Add(m_Collider2D, this);
    }

    private void OnEnable()
    {
        OnDeath += DestroyTower;
    }

    private void OnDisable()
    {
        OnDeath -= DestroyTower;
    }

    private void DestroyTower()
    {
        _playerTower.RemoveTowerUnit(this);
    }
}
