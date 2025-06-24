using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerType", menuName = "Scriptable Objects/EnemySpawnerType")]
public class EnemySpawnType : ScriptableObject
{
    public GameObject prefab;
    public float spawnCooldown = 5f;
    [Tooltip("Times for enemy to spawn. Zero or negative = infinite.")]
    public int maxCount = 0;
}
