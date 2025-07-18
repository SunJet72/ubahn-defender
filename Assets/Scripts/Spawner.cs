using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [SerializeField]
    private EnemySpawnType[] enemyTypes; // Array of enemy configs

    [SerializeField]
    private GameObject parentTransform;

    [Tooltip("If true, spawner will start spawning on Awake().")]
    [SerializeField]
    private bool startOnAwake = true;

    private int[] _spawnedCounts;
    private Coroutine _spawnRoutine;

    private void Awake()
    {
        _spawnedCounts = new int[enemyTypes.Length];

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
        while (true) // Infinite loop; exit condition below
        {
            bool allMaxed = true;

            for (int i = 0; i < enemyTypes.Length; i++)
            {
                var type = enemyTypes[i];

                // Check max spawn count per type
                if (type.maxCount > 0 && _spawnedCounts[i] >= type.maxCount)
                    continue;

                allMaxed = false;

                // Spawn enemy
                Instantiate(type.prefab, transform.position, transform.rotation, parentTransform?.transform);
                _spawnedCounts[i]++;

                // Wait for that type's cooldown
                yield return new WaitForSeconds(type.spawnCooldown);
            }

            if (allMaxed)
                break; // All max counts reached, exit loop
        }

        _spawnRoutine = null;
    }
}
