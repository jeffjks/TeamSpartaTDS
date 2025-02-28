using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHero : PlayerTowerUnit
{
    public PlayerBullet m_PlayerBullet;
    public float m_FireDelay;
    public Transform m_FirePosition;
    public Transform m_Gun;

    private float _currentFireDelay = 0f;

    private void Awake()
    {
        _playerTower = GetComponentInParent<PlayerTower>();
        CurrentHitPoint = m_HitPoint;
        HitPointManager.CachedHitPoint.Add(m_Collider2D, this);
    }

    private void Update()
    {
        AimGun();
        
        _currentFireDelay += Time.deltaTime;

        if (_currentFireDelay >= m_FireDelay)
        {
            _currentFireDelay = 0f;
            FireBullet();
        }
    }

    private void AimGun()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - m_Gun.transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        m_Gun.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void FireBullet()
    {
        Instantiate(m_PlayerBullet, m_FirePosition.position, m_FirePosition.rotation);
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
