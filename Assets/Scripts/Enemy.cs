using UnityEngine;
using UnityEngine.AI;
using Fusion;
using System;

public class Enemy : NetworkBehaviour
{
    [SerializeField]
    Transform target; // The target the enemy will follow
    [SerializeField] private int maxHealth = 50;
    [Networked]
    public int CurrentHealth { get; set; }
    [Networked, OnChangedRender(nameof(OnColorChanged))]
    public Color SpriteColor { get; set; }

    [Networked, OnChangedRender(nameof(OnMovementSpeedChanged))]
    public float MovementSpeed { get; set; }

    public bool HasSpawned { get; set; } = false;
    private SpriteRenderer spriteRenderer;
    NavMeshAgent agent;

    // public Transform ParentTransform { get; set; }

    public override void Spawned()
    {
        CurrentHealth = maxHealth;

        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = true; // Ensure the agent updates its position
        target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag

        HasSpawned = true;
    }
    void Update()
    {
        // if (transform.parent != ParentTransform)
        // {
        //     Debug.Log("No parent! Replace " + transform.parent?.name + " with " + ParentTransform);
        //     transform.localScale = Vector3.one;
        //     Debug.Log("Replaced parent: " + transform.parent?.name);
        // }
        agent.SetDestination(target.position); // Set the destination to the target's position
    }

    public void TakeDamage(int amount)
    {
        if (!HasSpawned)
        {
            Debug.Log("Tried to take damage while not spawned");
            return;
        }
        Debug.Log("Enemy took damage: " + CurrentHealth);
        SpriteColor = Color.red; // Change color to red when taking damage
        Invoke("ResetColor", 0.5f); // Reset color after a short delay

        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        Debug.Log("Enemy slowed by factor: " + slowFactor);
        SpriteColor = Color.cyan;
        MovementSpeed *= slowFactor; // Apply the slow effect by modifying the agent's speed
        Invoke("ResetSpeed", duration); // Reset speed after the duration
        Invoke("ResetColor", duration);
    }

    private void ResetSpeed()
    {
        MovementSpeed /= 0.5f; // Reset speed to original value (assuming slowFactor was 0.5)
        Debug.Log("Enemy speed reset");
    }

    private void ResetColor()
    {
        SpriteColor = Color.white; // Reset color to white
    }

    private void Die()
    {
        Runner.Despawn(Object);
    }

    private void OnColorChanged()
    {
        spriteRenderer.color = SpriteColor;
    }
    
    private void OnMovementSpeedChanged()
    {
        agent.speed = MovementSpeed;
    }
}
