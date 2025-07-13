using UnityEngine;
public enum EnemyType
{
    RANGED,
    MELEE,
    SCOUNDREL,
    TAXIST
}

[CreateAssetMenu(fileName = "EnemyCombatSystemData", menuName = "Scriptable Objects/EnemyCombatSystemData")]
public class EnemyCombatSystemData : UnitData
{
    public EnemyType enemyType;
}
