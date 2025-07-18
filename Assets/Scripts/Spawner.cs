using System;
using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    [SerializeField]
    private EnemySpawnType[] enemyTypes; // Array of enemy configs

    [SerializeField]
    private Transform parentTransform;

    [Tooltip("If true, spawner will start spawning on Awake().")]
    [SerializeField]
    private bool startOnAwake = true;

    private int[] _spawnedCounts;
    TickTimer _nextSpawn;

    public override void Spawned()
    {
        _spawnedCounts = new int[enemyTypes.Length];

        if(Runner.IsServer)
            StartSpawning();
    }

    public void StartSpawning()
    {
        if (_nextSpawn.ExpiredOrNotRunning(Runner))
            _nextSpawn = TickTimer.CreateFromSeconds(Runner, 1);
    }

    public void StopSpawning()
    {
        _nextSpawn = TickTimer.None;
    }
    private int i = 0;
    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer || !_nextSpawn.Expired(Runner)) return;
        bool allMaxed = true;

        if (i >= enemyTypes.Length) i = 0;
        for (; i < enemyTypes.Length; i++)
        {
            EnemySpawnType type = enemyTypes[i];

            // Check max spawn count per type
            if (type.maxCount > 0 && _spawnedCounts[i] >= type.maxCount)
                continue;

            allMaxed = false;

            // Spawn enemy
            NetworkObject obj = Runner.Spawn(type.prefab, transform.position, transform.rotation,
            onBeforeSpawned: (runner, spawned) =>
            {
                spawned.transform.SetParent(parentTransform);
                spawned.transform.localScale = Vector3.one;
            });
            _spawnedCounts[i]++;

            // Wait for that type's cooldown
            _nextSpawn = TickTimer.CreateFromSeconds(Runner, type.spawnCooldown);
        }

        if (allMaxed)
            StopSpawning();
    }
}
