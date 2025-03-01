using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public float m_MaxSpeedLeft;
    public float m_MaxSpeedRight;
    public float m_ForceForward;
    public float m_ForceToBelowEnemy;
    public float m_JumpForce;
    public float m_AttackRaycastDistance;
    public float m_FrontRaycastDistance;
    public int m_Damage;
    public float m_AttackDelay;
    public float m_JumpDelayMin, m_JumpDelayMax;
    public LayerMask m_PlayerLayer;
    public CapsuleCollider2D Collider2D;

    private EnemyZombie _enemyZombie;
    private Rigidbody2D _rigidBody;
    private bool _isGrounded = false;
    private bool _isOnCollider = false;
    private float _currentAttackDelay;
    private float _currentJumpDelay;
    private float _nextJumpDelay;

    private const float RaycastUpwardDistance = 10f;

    private void Start()
    {
        _enemyZombie = GetComponent<EnemyZombie>();
        _rigidBody = GetComponent<Rigidbody2D>();

        SetNextJumpDelay();
    }

    private void FixedUpdate()
    {
        OnGround();
        MoveForward();

        ValidateAttack();
        ValidateJump();

        SetMaxVelocity();
    }

    private void SetMaxVelocity()
    {
        var clampedVelocity = Mathf.Clamp(_rigidBody.velocity.x, m_MaxSpeedLeft, m_MaxSpeedRight);
        _rigidBody.velocity = new Vector2(clampedVelocity, _rigidBody.velocity.y);
    }

    private void MoveForward()
    {
        _rigidBody.AddForce(Vector2.right * m_ForceForward, ForceMode2D.Impulse);
    }

    private void ValidateAttack()
    {
        Vector2 origin = transform.position + Collider2D.transform.localPosition;
        var hit = Physics2D.BoxCast(origin, Collider2D.size, 0f, Vector2.left, m_AttackRaycastDistance, m_PlayerLayer);
        
        if (_currentAttackDelay < m_AttackDelay)
        {
            _currentAttackDelay += Time.deltaTime;
        }
        else if (hit.collider)
        {
            ExecuteAttack(hit.collider);
        }
    }

    private void ExecuteAttack(Collider2D collider2D)
    {
        _currentAttackDelay = 0f;
        HitPointManager.CachedHitPoint[collider2D].TakeDamage(m_Damage);
    }
    
    private void ValidateJump()
    {
        Vector2 origin = transform.position + Collider2D.transform.localPosition;
        var originFront = origin - new Vector2(Collider2D.size.x / 2f + 0.1f, 0f);

        var hitFront1 = Physics2D.Raycast(originFront, Vector2.left, m_FrontRaycastDistance, _enemyZombie.EnemyLayer);
        var hitFront2 = Physics2D.Raycast(originFront + new Vector2(0f, Collider2D.size.y), Vector2.left, m_FrontRaycastDistance, _enemyZombie.EnemyLayer);
        bool isFrontEnemyMoving = hitFront1 ? hitFront1.rigidbody.velocity.x < -0.5f : false;

        var hitUpward = Physics2D.Raycast(origin + new Vector2(0f, Collider2D.size.y), Vector2.up, RaycastUpwardDistance, _enemyZombie.EnemyLayer);

        var canClimb = hitFront1 && !hitFront2 && !isFrontEnemyMoving;
        var onGround = _isGrounded || _isOnCollider;
        
        if (_currentJumpDelay < _nextJumpDelay)
        {
            _currentJumpDelay += Time.deltaTime;
        }
        else if (canClimb && onGround && hitUpward == false)
        {
            ExecuteJump();
        }
    }

    private void ExecuteJump()
    {
        _currentJumpDelay = 0f;
        _rigidBody.AddForce(Vector2.up * m_JumpForce, ForceMode2D.Impulse);
        _isGrounded = false;
        _isOnCollider = false;
        SetNextJumpDelay();
    }

    private void OnGround()
    {
        if (transform.position.y <= _enemyZombie.GroundY)
        {
            transform.position = new Vector2(transform.position.x, _enemyZombie.GroundY);
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0f);
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    private void SetNextJumpDelay()
    {
        _nextJumpDelay = Random.Range(m_JumpDelayMin, m_JumpDelayMax);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.contacts[0].normal.y > 0.5f)
        {
            _isOnCollider = true;
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        foreach (ContactPoint2D contact in other.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                var zombieAI = _rigidBody.gameObject.GetComponent<ZombieAI>();
                if (zombieAI.IsGrounded() == false)
                    continue;
                _rigidBody.AddForce(Vector2.right * m_ForceToBelowEnemy, ForceMode2D.Impulse);
                
                break;
            }
        }
    }
}
