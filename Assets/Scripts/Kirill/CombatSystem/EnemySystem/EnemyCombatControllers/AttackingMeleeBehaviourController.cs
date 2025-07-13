using System;
using UnityEngine;

public class AttackingMeleeBehaviourController : CombatBehaviourController
{
    [SerializeField] private EnemyMeleeAttackData data;

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
        curAttackCooldown -= Time.fixedDeltaTime * Controller.AttackSpeed;

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
        transform.Translate((target.position - transform.position).normalized * Controller.Speed * Time.fixedDeltaTime);
    }

    private void Attack()
    {
        chaisedPlayer.Hurt(data.attackDamage);
        curAttackCooldown = 1f;
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
        Controller.MeleeLoseTarget();
    }

    public override void OnEndBehaviour()
    {
        chaisedPlayer = null;
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
