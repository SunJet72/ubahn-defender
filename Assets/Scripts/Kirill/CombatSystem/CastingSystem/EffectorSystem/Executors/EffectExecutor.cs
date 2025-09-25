using UnityEngine;

[CreateAssetMenu(fileName = "EffectExecutor", menuName = "Scriptable Objects/EffectExecutor")]
public abstract class EffectExecutor : ScriptableObject
{
    public abstract void ExecuteEffect(StatefulExecutor executor);
}
