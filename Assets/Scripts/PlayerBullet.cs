using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public Collider2D m_Collider2D;
    public int m_Damage;
    public float m_LifeTime;
    public float m_Speed;

    private float _currentLifeTime;

    private void Update()
    {
        _currentLifeTime += Time.deltaTime;

        if (_currentLifeTime >= m_LifeTime)
        {
            Destroy(gameObject);
        }

        MoveBullet();
    }

    private void MoveBullet()
    {
        transform.Translate(Vector2.right * m_Speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponentInParent<HasHitPoint>();
            enemy.TakeDamage(m_Damage);

            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}
