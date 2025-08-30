using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponGO : WeaponGO
{
    private List<EnemyCombatBehaviourSystem> nearestEnemies = new List<EnemyCombatBehaviourSystem>();
    private List<VehicleCombatBehaviourSystem> nearestVehicles = new List<VehicleCombatBehaviourSystem>();
    protected override void Attack(UnitController target)
    {
        Runner.Spawn(weapon.projectile, onBeforeSpawned: (runner, spawned) =>
        {
            spawned.transform.position = transform.position;
            player.ShootTarget(spawned.GetComponent<Projectile>(), target, weapon.damage);
        });
    }

    protected override UnitController FindTarget()
    {
        if (nearestEnemies.Count == 0 && nearestVehicles.Count == 0)
            return null;
        else // TODO: Make logic more complex, like dynamically set prefs (Attack vehicles, until its safe. If enemies are too close, switch on them)
        {
            if (nearestVehicles.Count == 0)
                return nearestEnemies[0];
            else
                return nearestVehicles[0];
        }
    }

    protected override void OnTiggerEnter2D(Collider2D collision)
    {
        var unit = collision.gameObject.GetComponent<UnitController>();
        if (unit != null)
        {
            if (unit is EnemyCombatBehaviourSystem enemy)
            {
                unit.OnDieEvent += LostEnemyTarget;
                nearestEnemies.Add(enemy);
            }
            if (unit is VehicleCombatBehaviourSystem vehicle)
            {
                unit.OnDieEvent += LostVehicleTarget;
                nearestVehicles.Add(vehicle);
            }
        }
    }

    protected override void OnTiggerExit2D(Collider2D collision)
    {
        var unit = collision.gameObject.GetComponent<UnitController>();
        if (unit != null)
        {
            if (unit is EnemyCombatBehaviourSystem enemy)
            {
                unit.OnDieEvent -= LostEnemyTarget;
                LostEnemyTarget(unit);
            }
            if (unit is VehicleCombatBehaviourSystem vehicle)
            {
                unit.OnDieEvent -= LostVehicleTarget;
                LostVehicleTarget(unit);
            }
        }
    }

    private void LostEnemyTarget(UnitController enemy)
    {
        nearestEnemies.Remove(enemy as EnemyCombatBehaviourSystem);
    }

    private void LostVehicleTarget(UnitController vehicle)
    {
        nearestVehicles.Remove(vehicle as VehicleCombatBehaviourSystem);
    }
}
