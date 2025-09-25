using Fusion;
using UnityEngine;

public class SpellEffect : InstantEffectExecutor
{
    public ActiveSpell spell;

    protected override void ApplyInstantEffect(StatefulExecutor executor)
    {
        spell.Activate(executor.GetCaster(), Vector2.zero);
    }
}
