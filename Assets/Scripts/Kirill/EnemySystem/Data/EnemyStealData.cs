using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData_NAME_Steal", menuName = "Combat/Enemy/Steal Data")]
public class EnemyStealData : ScriptableObject
{
    public float stealTime;
    public bool canBeCancelledByAttack;
}
