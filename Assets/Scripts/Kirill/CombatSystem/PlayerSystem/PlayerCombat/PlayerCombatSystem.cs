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

    private bool isSetUp = false;

    List<UnitType> unitTypesEnemyAndVehicle;

    [Networked]
    private NetworkObject spellArmor { get; set; }
    [Networked]
    private NetworkObject spellWeapon { get; set; }

    [Networked]
    private PlayerNetworkStruct networkData { get; set; }
    [Networked]
    private int armorId { get; set; }
    [Networked]
    private int weaponId { get; set; }

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerController PlayerController;


    public override void Spawned()
    {
        gameCombatManager = GameObject.Find("GameCombatManager").GetComponent<GameCombatManager>();

        unitTypesEnemyAndVehicle = new List<UnitType>
        {
            UnitType.ENEMY,
            UnitType.VEHICLE
        };
        //NetworkManager.Instance.InitPlayer(this);
    }

    public void AfterSpawned()
    {
        NetworkManager.Instance.InitPlayer(this);
        if (HasInputAuthority)
        {
            OnHealthChanged();
        }
    }

    public void Init(PlayerNetworkStruct data, int armorId, int weaponId)
    {
        if (!HasInputAuthority) return;

        this.networkData = data;
        this.armorId = armorId;
        this.weaponId = weaponId;
        this.consumables = new List<ScriptableConsumable>();

        this.data = networkData.CopyData();
        this.armorEq = (ScriptableArmor)ItemManager.instance.getItem(armorId);
        this.weaponEq = (ScriptableWeapon)ItemManager.instance.getItem(weaponId);
        this.consumables = new List<ScriptableConsumable>();

        SetCharacterSprite(true);

        base.Init();

        ApplyUnitDataStats(armorEq.unitData);
        ApplyUnitDataStats(weaponEq.unitData);

        //TODO Handle Consumables

        //
        isSetUp = true;
        InitSpellsRpc(Object.InputAuthority, armorId, weaponId);

        // Init(this.data, this.armorEq, this.weaponEq, this.consumables);

    }

    public void SetCharacterSprite(bool facingForward)
    {
        if (armorEq == null)
            return;
        if (facingForward)
            spriteRenderer.sprite = armorEq.PlayerSprite;
        else
            spriteRenderer.sprite = armorEq.PlayerBackSprite;
    }

    // public void Init(PlayerCombatSystemData data, ScriptableArmor armorEq, ScriptableWeapon weaponEq, List<ScriptableConsumable> consumables)
    // {
    //     this.data = data;
    //     this.armorEq = armorEq;
    //     this.weaponEq = weaponEq;
    //     this.consumables = new List<ScriptableConsumable>(consumables);

    //     // if (!didAwake)
    //     // {
    //     //     Awake();
    //     // }


    // }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void InitSpellsRpc(PlayerRef playerNO, int armorId, int weaponId)
    {
        ScriptableArmor localArmorEq = (ScriptableArmor)ItemManager.instance.getItem(armorId);
        ScriptableWeapon localWeaponEq = (ScriptableWeapon)ItemManager.instance.getItem(weaponId);
        if (Runner.IsServer)
        {
            spellArmor = Runner.Spawn(localArmorEq.spell, inputAuthority: playerNO, onBeforeSpawned: (runner, spawned) =>
            {
                spawned.transform.parent = transform;
                spawned.transform.localPosition = Vector2.zero;
            });

            spellWeapon = Runner.Spawn(localWeaponEq.spell, inputAuthority: playerNO, onBeforeSpawned: (runner, spawned) =>
            {
                spawned.transform.parent = transform;
                spawned.transform.localPosition = Vector2.zero;
            });

            if (spellArmor != null && spellWeapon != null) gameCombatManager.SetSpells(this, spellArmor.GetComponent<Spell>(), spellWeapon.GetComponent<Spell>());
        }
    }

    
    protected override void Die()
    {
        TriggerDeathEvent();

        gameObject.SetActive(false);
    }

    public void ShootTarget(Projectile projectile, UnitController target, float damage)
    {
        projectile.SetTarget(target.transform, this, CalculateDamage(weaponEq.damage), unitTypesEnemyAndVehicle);
    }

    private float CalculateDamage(float damage)
    {
        return damage * ((100f + Strength) / 100f);
    }

    public UnitController GetCurrentTargetSelected()
    {
        //return target;
        return null;
    }

    public override void OnHealthChanged()
    {
        if (Runner.GetPlayerObject(Runner.LocalPlayer).Equals(Object))
        {
            if (data == null) UIEvents.ShieldChanged((int)Health, 100);
            else UIEvents.ShieldChanged((int)Health, (int)data.health);
        }
    }

    public void SetupPlayerController(PlayerController playerController)
    {
        gameCombatManager.SetPlayerControls(playerController);
    }
}