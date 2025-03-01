using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyZombie m_Enemy;
    public int m_EnemyLayerIndex;

    public float m_SpawnDelayMin;
    public float m_SpawnDelayMax;

    private float _currentSpawnDelay;
    private float _nextSpawnDelay;
    
    private void Start()
    {
        _nextSpawnDelay = Random.Range(0f, m_SpawnDelayMax);
    }

    private void Update()
    {
        _currentSpawnDelay += Time.deltaTime;

        if (_currentSpawnDelay >= _nextSpawnDelay)
        {
            _currentSpawnDelay = 0f;
            SpawnEnemy();
            SetNextSpawnDelay();
        }
    }

    private void SetNextSpawnDelay()
    {
        _nextSpawnDelay = Random.Range(m_SpawnDelayMin, m_SpawnDelayMax);
    }

    private void SpawnEnemy()
    {
        var ins = Instantiate(m_Enemy, transform.position, Quaternion.identity);
        ins.GetComponent<ZombieAI>().SetEnemyLayerIndex(m_EnemyLayerIndex);
    }
}
