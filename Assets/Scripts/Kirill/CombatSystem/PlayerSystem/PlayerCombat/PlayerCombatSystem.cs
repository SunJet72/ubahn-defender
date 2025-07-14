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

    void Awake()
    {
        gameCombatManager = GameObject.Find("GameCombatManager").GetComponent<GameCombatManager>();
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
    protected override void Die()
    {
        throw new System.NotImplementedException();
    }
}
