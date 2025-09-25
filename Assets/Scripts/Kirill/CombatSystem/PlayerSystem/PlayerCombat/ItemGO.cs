using Fusion;
using UnityEngine;

public abstract class ItemGO : SpellCaster
{
    public abstract ScriptableItemBase ItemData { get; }

    private UnitController owner;

    public void Init(UnitController owner)
    {
        base.Init(ItemData.spell, owner);
        this.owner = owner;
    }
}
