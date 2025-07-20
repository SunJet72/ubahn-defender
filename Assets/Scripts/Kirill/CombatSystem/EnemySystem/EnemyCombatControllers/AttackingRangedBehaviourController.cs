using UnityEngine;
using System;
using Fusion;
using System.Collections.Generic;

public class AttackingRangedBehaviourController : CombatBehaviourController
{
    [SerializeField] private EnemyRangedAttackData data;

    private Transform target;
    private PlayerCombatSystem chaisedPlayer;
    private float curAttackCooldown = 0f;

    List<UnitType> unitTypes = new List<UnitType>();

    public void SetTarget(PlayerCombatSystem player)
    {
        chaisedPlayer = player;
        target = chaisedPlayer.gameObject.transform;
        player.OnDieEvent += OnPlayerKilled;

        if (unitTypes.Count == 0)
            unitTypes.Add(UnitType.PLAYER);
    }

    //---// Behaviour //---//
    public override void OnStartBehaviour()
    {
        if (target == null || chaisedPlayer == null)
        {
            Debug.LogError("I dont have a target to attack");
            return;
        }
    }

    public override void OnFixedUpdateBehave()
    {
        curAttackCooldown -= Runner.DeltaTime * Controller.AttackSpeed;

        if (curAttackCooldown <= 0)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (Runner.IsServer)
        {
            NetworkObject projectile = Runner.Spawn(data._projectile, onBeforeSpawned: (runner, spawned) =>
            {
                spawned.transform.position = transform.position;
                spawned.GetComponent<Projectile>().SetTarget(
                    (target.position - transform.position).normalized, Controller, CalculateDamage(data.damage), unitTypes);
            });
            
            curAttackCooldown = 1f;
        }
    }

    private void OnPlayerKilled(UnitController unit)
    {
        LoseTarget();
    }

    private void LoseTarget()
    {
        chaisedPlayer.OnDieEvent -= OnPlayerKilled;
        chaisedPlayer = null;
        target = null;
        Controller.RangedLoseTarget();
    }

    public override void OnEndBehaviour()
    {
        chaisedPlayer = null;
        target = null;
        curAttackCooldown = 1f;
    }

    private float CalculateDamage(float damage) {
        return damage * ((100f + damage) / 100f);
    }
}
