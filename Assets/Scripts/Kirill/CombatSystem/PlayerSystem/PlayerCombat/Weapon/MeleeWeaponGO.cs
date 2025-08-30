using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponGO : WeaponGO
{
    private List<EnemyCombatBehaviourSystem> nearestEnemies = new List<EnemyCombatBehaviourSystem>();
    protected override void Attack(UnitController target)
    {
        if (weapon.canDealSplashDamage)
        {
            // TODO: Add delay and visual interpretation of hit. For noe just do it instantly
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position, weapon.range);
            Vector2 direction = (target.transform.position - transform.position).normalized;
            foreach (var hit in hits)
            {
                Vector3 toTarget = hit.transform.position - transform.position;
                toTarget.z = 0;

                float angle = Vector3.Angle(direction, toTarget.normalized);
                if (angle <= weapon.fov / 2f)
                {
                    if (hit.TryGetComponent(out EnemyCombatBehaviourSystem enemy))
                    {
                        player.Hit(enemy, weapon.damage);
                    }
                }
            }
        }
        else
        {
            // TODO: Add delay and visual interpretation of hit. For noe just do it instantly
            player.Hit(target, weapon.damage);
        }
    }

    protected override UnitController FindTarget() // TODO: Make logic smarter, like choosing target, that is most benefitial (most around if splash damage)
    {
        if (nearestEnemies.Count == 0)
            return null;
        else
        {
            return nearestEnemies[0];
        }
    }

    protected override void OnTiggerEnter2D(Collider2D collision)
    {
        var unit = collision.gameObject.GetComponent<UnitController>();
        if (unit != null)
        {
            if (unit is EnemyCombatBehaviourSystem enemy)
            {
                unit.OnDieEvent += LostTarget;
                nearestEnemies.Add(enemy);
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
                unit.OnDieEvent -= LostTarget;
                LostTarget(unit);
            }
        }
    }

    private void LostTarget(UnitController enemy)
    {
        nearestEnemies.Remove(enemy as EnemyCombatBehaviourSystem);
    }
}
