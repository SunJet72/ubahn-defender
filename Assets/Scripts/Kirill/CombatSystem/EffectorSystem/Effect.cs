using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Scriptable Objects/Effect")]
public abstract class Effect : ScriptableObject
{
    public abstract EffectData EffectData { get; }
    public abstract void ApplyEffect(UnitController caster, UnitController target);
}
