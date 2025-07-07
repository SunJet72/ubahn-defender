using UnityEngine;

[CreateAssetMenu(fileName = "ShootingData_NAME_Projectile", menuName = "Combat/Shooting/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public float speed;
    public bool isSelfGuided;
    public float damage;
}
