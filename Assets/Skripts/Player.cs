using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Auto‐Attack Settings")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 50.0f;
    [SerializeField] private float attackRate = 1.0f;

   [Header("Ability C: AOE Damage")]
    
    [SerializeField] private int aoeDamage = 25;
    [SerializeField] private float aoeDamageRadius = 3f;
    [SerializeField] private float aoeCooldown = 6f;
    private float _nextAoeTime = 0f;

    [Header("Ability V: Icing (Slow)")]
    [Tooltip("How much to multiply enemy speed by (e.g. 0.5 means 50% speed).")]
    [Range(0f, 1f)]
    [SerializeField] private float slowFactor = 0.5f;
    [SerializeField] private float slowDuration = 4f;
    [SerializeField] private float slowRadius = 3f;
    [SerializeField] private float slowCooldown = 8f;
    private float _nextSlowTime = 0f;

    private float _nextAttackTime = 0f;


    private void Update()
    {
        AutoAttackNearestEnemy();

        if (Input.GetKeyDown(KeyCode.X))
            TryUseAbilityX();

        if (Input.GetKeyDown(KeyCode.C))
            TryUseAbilityC();
    }

    private void AutoAttackNearestEnemy()
    {
        if (Time.time < _nextAttackTime)
            return;

        // Find all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            return;

        // Identify the closest enemy
        GameObject nearest = null;
        float minDistSqr = Mathf.Infinity;
        Vector2 currentPos = transform.position;

        foreach (GameObject e in enemies)
        {
            float distSqr = ((Vector2)e.transform.position - currentPos).sqrMagnitude;
            if (distSqr < minDistSqr)
            {
                minDistSqr = distSqr;
                nearest = e;
            }
        }

        if (nearest == null)
            return;

        float distToNearest = Mathf.Sqrt(minDistSqr);
        // Only attack if within range
        if (distToNearest <= attackRange)
        {
            // Attempt to deal damage to the enemy’s health component
            Enemy enemyHealth = nearest.GetComponent<Enemy>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning($"Nearest enemy '{nearest.name}' has no EnemyHealth component.");
            }

            // Schedule next attack
            _nextAttackTime = Time.time + (1f / attackRate);
        }
    }

    private void TryUseAbilityX()
    {
        Debug.Log("Ability X used: AOE Damage");
        AoeDamage();
        _nextAoeTime = Time.time + aoeCooldown;
    }

     private void AoeDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aoeDamageRadius);
        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Enemy"))
            {
                Enemy eh = col.GetComponent<Enemy>();
                if (eh != null)
                    eh.TakeDamage(aoeDamage);
            }
        }

        // Optional visual feedback (e.g. flash or particle). Here, a quick debug circle:
        StartCoroutine(ShowAoeFlash(aoeDamageRadius, Color.red, 0.2f));
    }

    private IEnumerator ShowAoeFlash(float radius, Color color, float duration)
    {
        // Simple gizmo‐like flash: draw a circle for "duration" seconds
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.right * radius, color);
            Debug.DrawLine(transform.position, transform.position + Vector3.up * radius, color);
            yield return null;
        }
    }

    private void TryUseAbilityC()
    {
        Debug.Log("Ability C used: Icing (Slow)");
         IcingSlow();
        _nextSlowTime = Time.time + slowCooldown;
    }

    private void IcingSlow()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, slowRadius);
        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Enemy"))
            {
                Enemy em = col.GetComponent<Enemy>();
                if (em != null)
                {
                    em.ApplySlow(slowFactor, slowDuration);
                }
            }
        }

        // Optional: Visual feedback for slow radius (blue circle)
        StartCoroutine(ShowAoeFlash(slowRadius, Color.cyan, 0.2f));
    }
    

    private void OnDrawGizmosSelected()
    {
        // Visualize attackRange (red), AOE damage radius (yellow), and slow radius (cyan)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aoeDamageRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, slowRadius);
    }
}


