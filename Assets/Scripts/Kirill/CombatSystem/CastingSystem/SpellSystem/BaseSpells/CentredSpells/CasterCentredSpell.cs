using UnityEngine;

[CreateAssetMenu(fileName = "CasterCentredSpell", menuName = "Scriptable Objects/CasterCentredSpell")]
public class CasterCentredSpell : ActiveSpell
{
    [SerializeField] private ActiveSpellData data;
    public override SpellData SpellData => data;

    protected override Transform GetStartTransform(UnitController caster, Vector2 end)
    {
        return caster.transform;
    }
}
