using System.Collections.Generic;
using UnityEngine;
using Fusion;

public abstract class WeaponGO : NetworkBehaviour
{
    protected PlayerCombatSystem player;
    [SerializeField] protected ScriptableWeapon weapon;
    [SerializeField] private Spell spell;
    [SerializeField] private CircleCollider2D detectionCollider;

    private float cooldown;
    private bool isReadyToAttack;

    private UnitController curTarget;

    void Awake()
    {
        detectionCollider.radius = weapon.range;
    }

    public void Init(PlayerCombatSystem player)
    {
        this.player = player;
    }

    public override void FixedUpdateNetwork()
    {
        if (!isReadyToAttack)
        {
            cooldown -= Time.fixedDeltaTime;
            if (cooldown <= 0)
                isReadyToAttack = true;
        }
        if (isReadyToAttack)
        {
            if (curTarget == null)
            {
                curTarget = FindTarget();
                if (curTarget != null)
                {
                    Attack(curTarget);
                    cooldown = 1f / player.AttackSpeed;
                    isReadyToAttack = false;
                }
            }
        }
    }

    protected abstract void OnTiggerEnter2D(Collider2D collision);

    protected abstract void OnTiggerExit2D(Collider2D collision);

    protected abstract void Attack(UnitController target);
    protected abstract UnitController FindTarget();

    public Spell GetSpell()
    {
        return spell;
    }

    public UnitController GetCurrentTargetSelected()
    {
        return curTarget;
    }
}
