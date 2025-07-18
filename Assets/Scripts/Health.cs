using Fusion;
using UnityEngine;

public class Damageable : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth { get; set; }
    [Networked]
    public float CurrentHealth { get; set; }
    public override void Spawned()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        
    }
}
