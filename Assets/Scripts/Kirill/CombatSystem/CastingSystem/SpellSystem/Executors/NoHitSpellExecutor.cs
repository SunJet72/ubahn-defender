using System.Collections.Generic;
using UnityEngine;

public class NoHitSpellExecutor : SpellExecutor
{
    [SerializeField] private SpellExecutorData data;
    public override SpellExecutorData SpellExecutorData => data;

    protected override IEnumerable<UnitController> Hit(UnitController caster, Transform castTransform, Vector2 endPoint)
    {
        // DOES NOTHING
        yield break;
    }
}
