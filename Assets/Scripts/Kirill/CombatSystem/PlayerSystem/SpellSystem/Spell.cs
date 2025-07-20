using Fusion;
using UnityEngine;

public abstract class Spell : NetworkBehaviour
{
    public abstract SpellData SpellData { get; }
}
