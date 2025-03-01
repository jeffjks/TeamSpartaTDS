using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyZombie : HasHitPoint
{
    public static HashSet<EnemyZombie> EnemyZombies = new();

    public int m_EnemyLayerIndex;
    public SortingGroup m_SortingGroup;

    public LayerMask EnemyLayer {
        get;
        private set;
    }

    public float GroundY {
        get;
        private set;
    }

    private void Awake()
    {
        CurrentHitPoint = m_HitPoint;
    }

    private void Start()
    {
        SetEnemyLayerIndex(m_EnemyLayerIndex);
        SetSortingLayer(m_EnemyLayerIndex);
    }
    
    private void OnEnable()
    {
        EnemyZombies.Add(this);
    }

    private void OnDisable()
    {
        EnemyZombies.Remove(this);
    }

    private void Update()
    {
        Despawn();
    }

    private void Despawn()
    {
        if (transform.position.x < GameManager.Instance.m_DespawnX)
        {
            Destroy(gameObject);
        }
    }

    public void SetEnemyLayerIndex(int index)
    {
        m_EnemyLayerIndex = index;
        var layerName = $"Enemy{index + 1}";
        EnemyLayer = LayerMask.GetMask(layerName);
        SetLayer(layerName);
        GroundY = GameManager.Instance.m_GroundLayerY + GameManager.Instance.m_GroundLayerInterval * index;
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

    private void SetSortingLayer(int index)
    {
        m_SortingGroup.sortingOrder = -index;
    }
}
