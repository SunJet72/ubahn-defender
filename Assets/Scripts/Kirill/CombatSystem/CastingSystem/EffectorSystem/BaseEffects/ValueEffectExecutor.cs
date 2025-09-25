using UnityEngine;

[CreateAssetMenu(fileName = "ValueEffect", menuName = "Scriptable Objects/ValueEffect")]
public abstract class ValueEffectExecutor : EffectExecutor
{
    public override void ExecuteEffect(StatefulExecutor executor)
    {
        throw new System.NotImplementedException();
    }

    //protected abstract void TurnOffEffect();
    
    //protected abstract float ValueKoefFunction();
}
