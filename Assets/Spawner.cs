using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab; // Prefab to spawn
    [SerializeField]
    private GameObject parentTransform; // Prefab to spawn
    [SerializeField]
    private float spawnInterval = 5f; // Time interval between spawns
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    [Tooltip("Optional maximum number of enemies to spawn. Zero or negative = infinite.")]
    [SerializeField] private int maxSpawnCount = 0;

    [Tooltip("If true, spawner will start spawning on Awake(). Otherwise, call StartSpawning() manually.")]
    [SerializeField] private bool startOnAwake = true;

    private int _spawnedCount = 0;
    private Coroutine _spawnRoutine;

    private void Awake()
    {
        if (startOnAwake)
            StartSpawning();
    }

    public void StartSpawning()
    {
        if (_spawnRoutine == null)
            _spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        // Loop until we hit the max count (if specified), otherwise infinite
        while (maxSpawnCount <= 0 || _spawnedCount < maxSpawnCount)
        {
            SpawnEnemy();
            _spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }

        _spawnRoutine = null;
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner: No enemyPrefab assigned.", this);
            return;
        }

        // Instantiate at this spawner's position & rotation
        Instantiate(enemyPrefab, transform.position, transform.rotation, parentTransform?.transform);
    }

    // For visualization in Scene view:
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 0.5f);
    }
}
