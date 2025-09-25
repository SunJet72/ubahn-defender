using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : NetworkBehaviour
{
    protected UnitController caster;
    protected UnitController target;
    protected Vector2 direction;
    public virtual void Init(UnitController caster, UnitController initialTarget, Vector2 castedWorldPosition, Vector2 initialDirection) // initTarget may be null
    {
        this.caster = caster;
        target = initialTarget;
        transform.position = castedWorldPosition;

        direction = initialDirection;
        transform.up = direction;
    }
}
