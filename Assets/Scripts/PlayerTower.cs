using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    public Collider2D m_EnemyBorderCollider;
    private LinkedList<PlayerTowerUnit> _playerTowerUnits = new();

    private void Awake()
    {
        var playerUnits = GetComponentsInChildren<PlayerTowerUnit>();
        foreach (var item in playerUnits)
        {
            _playerTowerUnits.AddLast(item);
            //item.OnDeath += RemoveFromList;
        }

        UpdateTower();
    }

    public void RemoveTowerUnit(PlayerTowerUnit playerUnit)
    {
        _playerTowerUnits.Remove(playerUnit);
        UpdateTower();

        if (IsTowerDown())
        {
            DisableEnemyBorder();
        }
    }

    private void UpdateTower()
    {
        var curHeight = 0f;
        var curHeightLevel = 0;

        foreach (var item in _playerTowerUnits)
        {
            item.transform.localPosition = new Vector2(item.transform.localPosition.x, curHeight);
            item.m_HeightLevel = curHeightLevel;
            curHeight += item.m_Height;
            curHeightLevel++;
        }
    }

    private void DisableEnemyBorder()
    {
        m_EnemyBorderCollider.gameObject.SetActive(false);
    }

    private bool IsTowerDown()
    {
        return _playerTowerUnits.Count == 0;
    }
}
