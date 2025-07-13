using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData_NAME_MeleeAttack", menuName = "Combat/Enemy/Melee Attack Data")]
public class EnemyMeleeAttackData : ScriptableObject
{
    public float detectionRange;
    public float attackRange;
    public float attackDamage;
}
