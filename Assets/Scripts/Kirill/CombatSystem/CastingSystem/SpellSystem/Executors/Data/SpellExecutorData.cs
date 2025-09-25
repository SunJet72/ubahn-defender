using System.Collections.Generic;
using UnityEngine;

public class SpellExecutorData : ScriptableObject
{
    public List<Effect> OnBeginEffects;
    public List<Effect> OnHitEffects;
}
