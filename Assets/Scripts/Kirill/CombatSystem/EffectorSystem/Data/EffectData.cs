using System.Collections.Generic;
using UnityEngine;

public enum EffectTargetFilter
{
    SELF,
    ENEMIES,
    VEHICLES,
    ALLIES,
    
}

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
    public List<EffectTargetFilter> filters;
}
