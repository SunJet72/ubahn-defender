using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatSystem : UnitController, IAfterSpawned
{
    [SerializeField] private PlayerCombatSystemData data;
    protected override UnitData UnitData => data;

    private ScriptableArmor armorEq;
    private ScriptableWeapon weaponEq;
    private List<ScriptableConsumable> consumables;
    private GameCombatManager gameCombatManager;

    private float curAttackCooldown = 0f;
    private List<Transform> potentialTargets;
    private List<PlayerCombatSystem> nearestPlayers;
    private List<EnemyCombatBehaviourSystem> nearestEnemies;
    private List<VehicleCombatBehaviourSystem> nearestVehicles;

    [Networked]
    private NetworkObject spellArmor { get; set;}
    [Networked]
    private NetworkObject spellWeapon { get; set; }


    public override void Spawned()
    {
        gameCombatManager = GameObject.Find("GameCombatManager").GetComponent<GameCombatManager>();

        if (Runner.IsServer)
        {
            spellArmor = Runner.Spawn(armorEq.spell, inputAuthority: Object.InputAuthority, onBeforeSpawned: (runner, spawned) =>
            {
                spawned.transform.parent = transform;
                spawned.transform.localPosition = Vector2.zero;
            });

            spellWeapon = Runner.Spawn(weaponEq.spell, inputAuthority: Object.InputAuthority, onBeforeSpawned: (runner, spawned) =>
            {
                spawned.transform.parent = transform;
                spawned.transform.localPosition = Vector2.zero;
            });
        }

        potentialTargets = new List<Transform>();
        base.Init();
    }

    public void AfterSpawned()
    {
        if (HasInputAuthority)
        {
            gameCombatManager.SetSpells(spellArmor.GetComponent<Spell>(), spellWeapon.GetComponent<Spell>());
        }
    }

    public void Init(ScriptableArmor armorEq, ScriptableWeapon weaponEq, List<ScriptableConsumable> consumables)
    {
        this.armorEq = armorEq;
        this.weaponEq = weaponEq;
        this.consumables = new List<ScriptableConsumable>(consumables);

        // if (!didAwake)
        // {
        //     Awake();
        // }

        ApplyUnitDataStats(armorEq.unitData);
        ApplyUnitDataStats(weaponEq.unitData);

        //TODO Handle Consumables
    }

    void OnBecameVisible()
    {

        /*float distance = (transform.position - target.position).magnitude;
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
        }*/
    }

    public override void FixedUpdateNetwork()
    {
        curAttackCooldown -= Runner.DeltaTime * AttackSpeed;
        if (curAttackCooldown <= 0)
            Attack();

    }

    private void Attack()
    {
        switch (data.playerClass)
        {
            case PlayerClass.WARRIOR:
                break;
        } 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.gameObject.GetComponent<UnitController>();
        if (unit != null)
        {
            unit.OnDieEvent += UnitInRangeDied;
            if (unit is PlayerCombatSystem otherPlayer)
            {
                nearestPlayers.Add(otherPlayer);
                // TODO: For mothers arm and other interaction with players
            }
            if (unit is EnemyCombatBehaviourSystem enemy)
            {
                nearestEnemies.Add(enemy);
            }
            if (unit is VehicleCombatBehaviourSystem vehicle)
            {
                nearestVehicles.Add(vehicle);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        var unit = collision.gameObject.GetComponent<UnitController>();
        if (unit != null)
        {
            unit.OnDieEvent -= UnitInRangeDied;
            if (unit is PlayerCombatSystem otherPlayer)
            {
                nearestPlayers.Remove(otherPlayer);
                // TODO: For mothers arm and other interaction with players
            }
            if (unit is EnemyCombatBehaviourSystem enemy)
            {
                nearestEnemies.Remove(enemy);
            }
            if (unit is VehicleCombatBehaviourSystem vehicle)
            {
                nearestVehicles.Remove(vehicle);
            }
        }
    }

    private void UnitInRangeDied(UnitController unit)
    {
        
    }
    protected override void Die()
    {
        throw new System.NotImplementedException();
    }
}
