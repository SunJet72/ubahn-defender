using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatSystem : UnitController, IAfterSpawned
{
    private PlayerCombatSystemData data;
    public override UnitData UnitData => data;

    private ScriptableArmor armorEq;
    private ScriptableWeapon weaponEq;
    private List<ScriptableConsumable> consumables;
    private GameCombatManager gameCombatManager;

    private List<PlayerCombatSystem> nearestPlayers = new List<PlayerCombatSystem>();
    private List<EnemyCombatBehaviourSystem> nearestEnemies = new List<EnemyCombatBehaviourSystem>();
    private List<VehicleCombatBehaviourSystem> nearestVehicles = new List<VehicleCombatBehaviourSystem>();

    private UnitController target;
    private float curAttackCooldown = 0f;
    private bool isSetUp = false;

    [SerializeField] private CircleCollider2D detectionCollider;

    List<UnitType> unitTypesEnemy;
    List<UnitType> unitTypesVehicle;
    List<UnitType> unitTypesEnemyAndVehicle;

    [Networked]
    private NetworkObject spellArmor { get; set; }
    [Networked]
    private NetworkObject spellWeapon { get; set; }


    public override void Spawned()
    {
        gameCombatManager = GameObject.Find("GameCombatManager").GetComponent<GameCombatManager>();

        unitTypesEnemy = new List<UnitType>
        {
            UnitType.ENEMY
        };
        unitTypesVehicle = new List<UnitType>
        {
            UnitType.VEHICLE
        };
        unitTypesEnemyAndVehicle = new List<UnitType>
        {
            UnitType.ENEMY,
            UnitType.VEHICLE
        };

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
            base.Init();
        }

    }

    public void AfterSpawned()
    {
        if (HasInputAuthority)
        {
            gameCombatManager.SetSpells(this, spellArmor.GetComponent<Spell>(), spellWeapon.GetComponent<Spell>());
            OnHealthChanged();
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.InputAuthority)]
    public void InitRpc(PlayerNetworkStruct data, int armorId, int weaponId)
    {
        this.data = data.CopyData();
        this.armorEq = (ScriptableArmor) ItemManager.instance.getItem(armorId);
        this.weaponEq = (ScriptableWeapon) ItemManager.instance.getItem(weaponId);
        this.consumables = new List<ScriptableConsumable>();

        Init(this.data, this.armorEq, this.weaponEq, this.consumables);
    }

    public void Init(PlayerCombatSystemData data, ScriptableArmor armorEq, ScriptableWeapon weaponEq, List<ScriptableConsumable> consumables)
    {
        this.data = data;
        this.armorEq = armorEq;
        this.weaponEq = weaponEq;
        this.consumables = new List<ScriptableConsumable>(consumables);

        // if (!didAwake)
        // {
        //     Awake();
        // }

        ApplyUnitDataStats(armorEq.unitData);
        ApplyUnitDataStats(weaponEq.unitData);

        detectionCollider.radius = weaponEq.range;

        //TODO Handle Consumables

        //
        isSetUp = true;
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
        if (!Runner.IsServer) return;
        if (!isSetUp)
            return;
        curAttackCooldown -= Runner.DeltaTime * AttackSpeed;
        if (curAttackCooldown <= 0)
            TryAttack();

    }

    private void TryAttack()
    {
        switch (data.playerClass)
        {
            case PlayerClass.WARRIOR:
                if (nearestEnemies.Count == 0)
                    return;
                if (target == null || (target.transform.position - transform.position).magnitude > weaponEq.range)
                {
                    target = FindNearestUnit(nearestEnemies);
                    AttackMelee();
                }
                else
                {
                    AttackMelee();
                }
                break;
            case PlayerClass.RANGER: // Fon now just attack vehicles, but has to be added additional logic, if some enemies are really close
                if (nearestVehicles.Count == 0)
                    return;
                if (target == null || (target.transform.position - transform.position).magnitude > weaponEq.range)
                {
                    target = FindNearestUnit(nearestVehicles);
                    if (target == null)
                        return;
                    AttackRanged();
                }
                else
                {
                    AttackRanged();
                }
                break;
            case PlayerClass.INGENIEUR: // The same with Ranger, but also adjust for mothers arm
                if (nearestVehicles.Count == 0)
                    return;
                if (target == null || (target.transform.position - transform.position).magnitude > weaponEq.range)
                {
                    target = FindNearestUnit(nearestVehicles);
                    if (target == null)
                        return;
                    AttackRanged();
                }
                else
                {
                    AttackRanged();
                }
                break;
        }
    }

    private void AttackMelee()
    {
        if (weaponEq.canDealSplashDamage)
        {
            // TODO: Add delay and visual interpretation of hit. For noe just do it instantly
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position, weaponEq.range);
            Vector2 direction = (target.transform.position - transform.position).normalized;
            foreach (var hit in hits)
            {
                Vector3 toTarget = hit.transform.position - transform.position;
                toTarget.z = 0;

                float angle = Vector3.Angle(direction, toTarget.normalized);
                if (angle <= weaponEq.fov / 2f)
                {
                    if (hit.TryGetComponent(out EnemyCombatBehaviourSystem enemy))
                    {
                        enemy.Hurt(CalculateDamage(weaponEq.damage), ArmorPenetration, this);
                    }
                }
            }
        }
        else
        {
            // TODO: Add delay and visual interpretation of hit. For noe just do it instantly
            target.Hurt(CalculateDamage(weaponEq.damage), ArmorPenetration, this);
        }
        curAttackCooldown = weaponEq.cooldown;
    }
    // [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void AttackRanged()
    {
        Runner.Spawn(weaponEq.projectile, onBeforeSpawned: (runner, spawned) =>
        {
            spawned.transform.position = transform.position;
            spawned.GetComponent<Projectile>().SetTarget(target.transform, this, CalculateDamage(weaponEq.damage), unitTypesEnemyAndVehicle);
        });

        // Later on make logic for whict type of enemies to attack

        curAttackCooldown = weaponEq.cooldown;
    }
    void OnTriggerEnter2D(Collider2D collision) // Can get wrong if the were in the radius from start on?
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
            RemoveUnitFromNearestList(unit);
        }
    }

    private void UnitInRangeDied(UnitController unit)
    {
        RemoveUnitFromNearestList(unit);
    }

    private void RemoveUnitFromNearestList(UnitController unit)
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
            Debug.LogWarning("I am removing Vehicle from nearest vehicles " + vehicle);
            nearestVehicles.Remove(vehicle);
            Debug.Log("Current amount of vehicles in nearest list: " + nearestVehicles.Count);
        }
    }

    private UnitController FindNearestUnit<T>(List<T> units) where T : UnitController
    {
        float curDistance = float.MaxValue;
        UnitController choseEnemy = null;
        foreach (var unit in units)
        {
            if (unit == null)
                continue; // Hardcoded! It cant be null, but is.
            float distance = (unit.transform.position - transform.position).magnitude;
            if (distance < curDistance)
            {
                curDistance = distance;
                choseEnemy = unit;
            }
        }
        if (choseEnemy == null)
        {
            return null;
        }
        return choseEnemy;
    }
    protected override void Die()
    {
        TriggerDeathEvent();

        gameObject.SetActive(false);
    }

    private float CalculateDamage(float damage)
    {
        return damage * ((100f + Strength) / 100f);
    }

    public UnitController GetCurrentTargetSelected()
    {
        return target;
    }

    public override void OnHealthChanged()
    {
        if (Runner.GetPlayerObject(Runner.LocalPlayer).Equals(Object))
        {
            if (data == null) UIEvents.ShieldChanged((int)Health, 100);
            else UIEvents.ShieldChanged((int)Health, (int)data.health);
        }
    }
}
