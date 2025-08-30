using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpellExecutorData", menuName = "Scriptable Objects/ProjectileSpellExecutorData")]
public class ProjectileSpellExecutorData : SpellExecutorData
{
    public TargetType targetType; // Has to be the same with the Projectile!
    public List<UnitType> targetTypes;

    public float flightSpeed;
    public float maxDistance;
    public GameObject _projectile;
}
