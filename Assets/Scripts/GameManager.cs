using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float m_GroundLayerY;
    public float m_GroundLayerInterval;
    public float m_EnemyBorderX;
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }
        Instance = this;
    }
}

public abstract class PlayerTowerUnit : HasHitPoint
{
    public float m_Height;
    public int m_HeightLevel;
    public Collider2D m_Collider2D;

    protected PlayerTower _playerTower;
}
