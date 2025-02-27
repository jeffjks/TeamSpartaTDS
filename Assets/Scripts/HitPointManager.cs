using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HasHitPoint : MonoBehaviour
{
    public int m_HitPoint;
    public event Action OnCurrentHitPointUpdated;
    public event Action OnTakeDamage;
    public event Action OnDeath;
    
    public int CurrentHitPoint {
        get {
            return _currentHitPoint;
        }
        set {
            _currentHitPoint = value;
            OnCurrentHitPointUpdated?.Invoke();
        }
    }
    protected int _currentHitPoint;

    public virtual void TakeDamage(int damage)
    {
        OnTakeDamage?.Invoke();
        CurrentHitPoint = Mathf.Max(0, CurrentHitPoint - damage);
        if (CurrentHitPoint <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}

public class HitPointManager : MonoBehaviour
{
    public static Dictionary<Collider2D, HasHitPoint> CachedHitPoint = new();
}
