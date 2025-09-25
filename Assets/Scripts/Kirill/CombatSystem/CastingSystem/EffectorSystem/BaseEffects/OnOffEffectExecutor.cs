using UnityEngine;

[CreateAssetMenu(fileName = "OnOffEffect", menuName = "Scriptable Objects/OnOffEffect")]
public abstract class OnOffEffectExecutor : EffectExecutor
{
    [SerializeField] private float duration;
    public override void ExecuteEffect(StatefulExecutor executor)
    {
        var act = executor.SetNormalTimer(duration);
        act += TurnOffEffect;
        TurnOnEffect(executor);
    }

    protected abstract void TurnOnEffect(StatefulExecutor executor);
    protected abstract void TurnOffEffect(StatefulExecutor executor);
}
