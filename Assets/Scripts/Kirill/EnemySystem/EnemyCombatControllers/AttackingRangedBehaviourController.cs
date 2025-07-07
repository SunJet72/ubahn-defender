using UnityEngine;
using System;

public class AttackingRangedBehaviourController : CombatBehaviourController
{
    [SerializeField] private EnemyRangedAttackData data;

    private Transform target;
    private PlayerMock chaisedPlayer;
    private float curAttackCooldown = 0f;

    public void SetTarget(PlayerMock player)
    {
        chaisedPlayer = player;
        target = chaisedPlayer.gameObject.transform;
        player.onDieEvent += OnPlayerKilled;
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
        curAttackCooldown -= Time.fixedDeltaTime;

        if (curAttackCooldown <= 0)
        {
            Attack();
        }
    }

    private void Attack()
    {
        var projectile = Instantiate(data._projectile);
        projectile.transform.position = transform.position;
        projectile.GetComponent<Projectile>().SetTarget(target);
        curAttackCooldown = data.timeBetweenAttacks;
    }

    private void OnPlayerKilled(System.Object obj, EventArgs e)
    {
        LoseTarget();
    }

    private void LoseTarget()
    {
        chaisedPlayer.onDieEvent -= OnPlayerKilled;
        chaisedPlayer = null;
        target = null;
        Controller.RangedLoseTarget();
    }

    public override void OnEndBehaviour()
    {
        chaisedPlayer = null;
        target = null;
        curAttackCooldown = data.timeBetweenAttacks;
    }
}
