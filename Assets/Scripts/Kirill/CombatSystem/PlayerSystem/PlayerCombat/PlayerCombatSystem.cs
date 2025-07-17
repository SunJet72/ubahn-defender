using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatSystem : UnitController
{
    protected override UnitData UnitData => throw new System.NotImplementedException();

    private ScriptableArmor armor;
    private ScriptableWeapon weapon;
    private List<ScriptableConsumable> consumables;

    public override void Spawned()
    {
        base.Init();
    }

    public void Init(ScriptableArmor armor, ScriptableWeapon weapon, List<ScriptableConsumable> consumables)
    {
        this.armor = armor;
        this.weapon = weapon;
        this.consumables = new List<ScriptableConsumable>(consumables);
    }
    protected override void Die()
    {
        throw new System.NotImplementedException();
    }
}
