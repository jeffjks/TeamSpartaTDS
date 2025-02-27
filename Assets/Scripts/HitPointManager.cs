using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HasHitPoint : MonoBehaviour
{
    public int m_HitPoint;
    public UnityAction m_UA_OnCurrentHitPointUpdated;
    public UnityAction m_UA_OnTakeDamage;
    
    public int CurrentHitPoint {
        get {
            return _currentHitPoint;
        }
        set {
            _currentHitPoint = value;
            m_UA_OnCurrentHitPointUpdated?.Invoke();
        }
    }
    protected int _currentHitPoint;

    public virtual void TakeDamage(int damage)
    {
        m_UA_OnTakeDamage?.Invoke();
        CurrentHitPoint = Mathf.Max(0, CurrentHitPoint - damage);
        if (CurrentHitPoint <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}

public class HitPointManager : MonoBehaviour
{
    public static Dictionary<Collider2D, HasHitPoint> CachedHitPoint = new();
}
