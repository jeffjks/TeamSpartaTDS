using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HasHitPoint : MonoBehaviour
{
    public int m_HitPoint;
    
    public int CurrentHitPoint {
        get {
            return _currentHitPoint;
        }
        set {
            _currentHitPoint = value;
            OnCurrentHitPointUpdated();
        }
    }
    protected int _currentHitPoint;

    public virtual void TakeDamage(int damage)
    {
        CurrentHitPoint = Mathf.Max(0, CurrentHitPoint - damage);
        if (CurrentHitPoint <= 0)
        {
            Death();
        }
    }

    protected virtual void OnCurrentHitPointUpdated() {}

    private void Death()
    {
        Destroy(gameObject);
    }
}

public class HitPointManager : MonoBehaviour
{
    public static Dictionary<Collider2D, HasHitPoint> CachedHitPoint = new();
}
