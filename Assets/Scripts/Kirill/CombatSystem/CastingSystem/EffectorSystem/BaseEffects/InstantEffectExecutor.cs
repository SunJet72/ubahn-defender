using UnityEngine;

[CreateAssetMenu(fileName = "InstantEffect", menuName = "Scriptable Objects/InstantEffect")]
public abstract class InstantEffectExecutor : EffectExecutor
{
    public override void ExecuteEffect(StatefulExecutor executor)
    {
        ApplyInstantEffect(executor);
    }

    protected abstract void ApplyInstantEffect(StatefulExecutor executor);
}
