using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombie : HasHitPoint
{
    public static HashSet<EnemyZombie> EnemyZombies = new();

    private void Awake()
    {
        CurrentHitPoint = m_HitPoint;
    }
    
    private void OnEnable()
    {
        EnemyZombies.Add(this);
    }

    private void OnDisable()
    {
        EnemyZombies.Remove(this);
    }
}
