using UnityEngine;

public class NoHitSpellExecutor : SpellExecutor
{
    [SerializeField] private SpellExecutorData data;
    public override SpellExecutorData SpellExecutorData => data;

    protected override void Hit()
    {
        // Does nothing, since it doesnt hit
    }
}
