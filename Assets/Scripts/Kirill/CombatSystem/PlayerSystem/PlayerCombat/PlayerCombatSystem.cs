using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatSystem : UnitController
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

    void Awake()
    {
        gameCombatManager = GameObject.Find("GameCombatManager").GetComponent<GameCombatManager>();
        potentialTargets = new List<Transform>();
        base.Init();
    }

    public void Init(ScriptableArmor armorEq, ScriptableWeapon weaponEq, List<ScriptableConsumable> consumables)
    {
        this.armorEq = armorEq;
        this.weaponEq = weaponEq;
        this.consumables = new List<ScriptableConsumable>(consumables);

        if (!didAwake)
        {
            Awake();
        }

        ApplyUnitDataStats(armorEq.unitData);
        ApplyUnitDataStats(weaponEq.unitData);

        var spellArmorGO = Instantiate(armorEq.spell);
        spellArmorGO.transform.parent = transform;
        spellArmorGO.transform.localPosition = Vector2.zero;
        Spell spellArmor = spellArmorGO.GetComponent<Spell>();

        var spellWeaponGO = Instantiate(weaponEq.spell);
        spellWeaponGO.transform.parent = transform;
        spellWeaponGO.transform.localPosition = Vector2.zero;
        Spell spellWeapon = spellWeaponGO.GetComponent<Spell>();

        gameCombatManager.SetSpells(spellArmor, spellWeapon);

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

    void FixedUpdate()
    {
        curAttackCooldown -= Time.fixedDeltaTime * AttackSpeed;
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
