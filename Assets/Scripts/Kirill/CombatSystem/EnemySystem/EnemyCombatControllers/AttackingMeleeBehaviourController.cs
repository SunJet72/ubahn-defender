using System;
using UnityEngine;

public class AttackingMeleeBehaviourController : CombatBehaviourController
{
    [SerializeField] private EnemyMeleeAttackData data;

    private Transform target;
    private PlayerCombatSystem player;
    private float curAttackCooldown = 0f;

    public void SetTarget(PlayerCombatSystem player)
    {
        this.player = player;
        target = this.player.gameObject.transform;
        player.OnDieEvent += OnPlayerKilled;
    }

    //---// Behaviour //---//
    public override void OnStartBehaviour()
    {
        Debug.Log("Tarrget: " + target + " Player: " + player);
        if (target == null || player == null)
        {
            Debug.LogError("I dont have a target to attack");
            return;
        }
    }

    public override void OnFixedUpdateBehave()
    {
        curAttackCooldown -= Runner.DeltaTime * Controller.AttackSpeed;

        float distance = (transform.position - target.position).magnitude;
        if (data.detectionRange < distance)
        {
            LoseTarget();
        }
        else if (data.attackRange < distance)
        {
            Chaise();
        }
        else
        {
            if (curAttackCooldown <= 0)
            {
                Attack();
            }
        }
    }

    private void Chaise()
    {
        transform.Translate((target.position - transform.position).normalized * Controller.Speed * Runner.DeltaTime);
    }

    private void Attack()
    {
        Controller.Hit(player, data.attackDamage);
        curAttackCooldown = 1f;
    }

    private void OnPlayerKilled(UnitController unit)
    {
        LoseTarget();
    }

    private void LoseTarget()
    {
        player.OnDieEvent -= OnPlayerKilled;
        player = null;
        target = null;
        Controller.MeleeLoseTarget();
    }

    public override void OnEndBehaviour()
    {
        Debug.Log("I am ending this behaviour");
        player = null;
        target = null;
        curAttackCooldown = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.detectionRange);
    }
}
