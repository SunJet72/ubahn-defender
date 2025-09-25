using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class SpellExecutor
{
    public abstract SpellExecutorData SpellExecutorData { get; }
    public IEnumerable<UnitController> Perform(UnitController caster, Transform castTransform, Vector2 endPoint)
    {
        return Hit(caster, castTransform, endPoint);
    }

    protected abstract IEnumerable<UnitController> Hit(UnitController caster, Transform castTransform, Vector2 endPoint);
}
