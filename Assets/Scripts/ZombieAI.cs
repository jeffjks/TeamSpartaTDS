using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public float m_MoveSpeed;      // 좀비의 이동 속도
    public float m_JumpForce;      // 점프 힘
    public float m_AttackRaycastDistance;
    public float m_FrontRaycastDistance;
    public int m_Damage;
    public float m_AttackDelay;
    public float m_JumpDelayMin, m_JumpDelayMax;
    public LayerMask m_PlayerLayer;
    public CapsuleCollider2D Collider2D;
    public int m_EnemyLayerIndex;

    private Rigidbody2D _rigidBody;
    private bool _isGrounded = false;
    private bool _isOnCollider = false;
    private bool _checkForward1 = false;
    private bool _checkForward2 = false;
    private bool _checkUpward = false;
    private float _currentAttackDelay;
    private float _currentJumpDelay;
    private float _nextJumpDelay;
    private LayerMask _enemyLayer;
    private float _groundY;

    private const float RaycastUpwardDistance = 10f;
    private const float Padding = 0.1f;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        SetNextJumpDelay();

        SetEnemyLayerIndex(m_EnemyLayerIndex);
    }

    private void FixedUpdate()
    {
        OnGround();
        MoveForward();

        ValidateAttack();
        ValidateJump();
        //Debug.DrawLine(originFront, originFront + new Vector2(-m_FrontRaycastDistance, 0f));
        //Debug.DrawLine(originFront + new Vector2(0f, Collider2D.size.y), originFront + new Vector2(-m_FrontRaycastDistance, Collider2D.size.y));
    }

    private void MoveForward()
    {
        _rigidBody.velocity = new Vector2(m_MoveSpeed, _rigidBody.velocity.y);
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

        _checkForward1 = Physics2D.Raycast(originFront, Vector2.left, m_FrontRaycastDistance, _enemyLayer);
        _checkForward2 = Physics2D.Raycast(originFront + new Vector2(0f, Collider2D.size.y), Vector2.left, m_FrontRaycastDistance, _enemyLayer);
        Debug.DrawLine(originFront + new Vector2(0f, Collider2D.size.y), originFront + new Vector2(-m_FrontRaycastDistance, Collider2D.size.y));

        var canClimb = _checkForward1 && !_checkForward2;
        var onGround = _isGrounded || _isOnCollider;
        
        if (_currentJumpDelay < _nextJumpDelay)
        {
            _currentJumpDelay += Time.deltaTime;
        }
        else if (canClimb && onGround && _checkUpward == false)
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

    // 바닥에 닿았을 때
    private void OnGround()
    {
        if (transform.position.y <= _groundY)
        {
            transform.position = new Vector2(transform.position.x, _groundY);
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0f); // 아래로 떨어지지 않음
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    public void SetEnemyLayerIndex(int index)
    {
        m_EnemyLayerIndex = index;
        var layerName = $"Enemy{index + 1}";
        _enemyLayer = LayerMask.GetMask(layerName);
        SetLayer(layerName);
        _groundY = GroundManager.GroundLayerY + GroundManager.GroundLayerInterval * index;
    }

    public void SetLayer(string name)
    {
        SetLayersRecursively(transform, name);
    }
    
    public void SetLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach(Transform child in trans)
        {
            SetLayersRecursively(child, name);
        }
    }

    private void SetNextJumpDelay()
    {
        _nextJumpDelay = Random.Range(m_JumpDelayMin, m_JumpDelayMax);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            _isOnCollider = true;
        }

        // TODO. OnCollisionStay2D 확인. 아래의 충돌한 물체가 있으면 아래 물체를 오른쪽으로 AddForce 하는 방법 체크
        // 충돌한 물체가 위에서 눌렀는지 확인
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // 충돌 각도가 위쪽에서 눌렀을 때만 적용
            if (contact.normal.y < -0.5f)
            {
                Vector2 pushDirection = Vector2.right;
                
                // 옆으로 밀리는 Force 적용 (Impulse 모드)
                _rigidBody.AddForce(pushDirection * 5f, ForceMode2D.Impulse);

                Debug.Log("위에서 눌림: " + collision.collider.name);
                break;  // 여러 접점 중 하나만 적용
            }
        }
    }
}
