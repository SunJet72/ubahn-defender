using UnityEngine;

[CreateAssetMenu(fileName = "Filter", menuName = "Scriptable Objects/Filter")]
public abstract class Filter : ScriptableObject
{
    public abstract bool Check(UnitController caster, UnitController target);
}
