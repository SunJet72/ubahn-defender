using UnityEngine;
using UnityEditor.AI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Transform target; // The target the enemy will follow
    [SerializeField] private int maxHealth = 50;
    public int currentHealth;
    private SpriteRenderer spriteRenderer;
    NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = true; // Ensure the agent updates its position
        Debug.Log(agent.updateRotation);
        target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position); // Set the destination to the target's position
    }


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Enemy took damage: " + currentHealth);
        spriteRenderer.color = Color.red; // Change color to red when taking damage
        Invoke("ResetColor", 0.5f); // Reset color after a short delay

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        Debug.Log("Enemy slowed by factor: " + slowFactor);
        spriteRenderer.color = Color.cyan;
        agent.speed *= slowFactor; // Apply the slow effect by modifying the agent's speed
        Invoke("ResetSpeed", duration); // Reset speed after the duration
        Invoke("ResetColor", duration);
  }

    private void ResetSpeed()
    {
        agent.speed = agent.speed / 0.5f; // Reset speed to original value (assuming slowFactor was 0.5)
        Debug.Log("Enemy speed reset");
    }

    private void ResetColor()
    {
        spriteRenderer.color = Color.white; // Reset color to white
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
