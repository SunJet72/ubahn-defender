using Fusion;
using UnityEngine;

public class SpellEffect : Effect
{
    public ActiveSpell spell;

    public override EffectData EffectData => throw new System.NotImplementedException();

    public override void ApplyEffect(UnitController caster, UnitController target)
    {
        spell.Activate(caster.Object, caster.GetComponent<NetworkObject>(), target.transform.position);
    }
}
