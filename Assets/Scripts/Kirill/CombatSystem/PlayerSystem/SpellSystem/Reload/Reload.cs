using System;
using Fusion;
using UnityEngine;

public abstract class Reload : NetworkBehaviour
{
    public event Action OnReloadedEvent;
    public abstract float ReloadPercentage { get; }
    public abstract void SpellWasUsed();
    public abstract bool IsReady();
    protected void Trigger()
    {
        OnReloadedEvent.Invoke();
    }
    
}