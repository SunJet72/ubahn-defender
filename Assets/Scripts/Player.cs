using System;
using System.Collections;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;

public class Player : NetworkBehaviour
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

    [Networked] private TickTimer autoAttackCooldown { get; set; }
    [Networked] private TickTimer xAbilityCooldown { get; set; }

    [Networked] private TickTimer cAbilityCooldown { get; set; }

    [Networked, OnChangedRender(nameof(OnColorChanged))] public Color SpriteColor { get; set; }

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    public SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Spawned()
    {
        OnColorChanged();
        if (HasInputAuthority)
        {
            FindFirstObjectByType<CinemachineCamera>().Target.TrackingTarget = transform;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            if (GetInput(out NetworkInputData data))
            {
                // store latest input as 'previous' state we had
                if (data.Buttons.WasPressed(ButtonsPrevious, NetworkInputData.X))
                {
                    TryUseAbilityX();
                }
                if (data.Buttons.WasPressed(ButtonsPrevious, NetworkInputData.C))
                {
                    TryUseAbilityC();
                }
                ButtonsPrevious = data.Buttons;
            }
            AutoAttackNearestEnemy();
        }
    }

    private void AutoAttackNearestEnemy()
    {
        if (!autoAttackCooldown.ExpiredOrNotRunning(Runner))
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
            if (distSqr < minDistSqr && e.GetComponent<Enemy>().HasSpawned)
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
            if (!enemyHealth.HasSpawned)
                Debug.Log("Enemy didnt spawn yet");
            if (enemyHealth != null && enemyHealth.HasSpawned)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
                else
                {
                    Debug.LogWarning($"Nearest enemy '{nearest.name}' has no EnemyHealth component.");
                }

            // Schedule next attack
            // _nextAttackTime = Time.time + (1f / attackRate);
            autoAttackCooldown = TickTimer.CreateFromSeconds(Runner, 1f / attackRate);
        }
    }

    private void TryUseAbilityX()
    {
        if (!xAbilityCooldown.ExpiredOrNotRunning(Runner))
        {
            Debug.Log("Ability X is in cooldown");
            return;
        }
        Debug.Log("Ability X used: AOE Damage");
        AoeDamage();
        xAbilityCooldown = TickTimer.CreateFromSeconds(Runner, aoeCooldown);
    }

    private void AoeDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aoeDamageRadius);
        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Enemy"))
            {
                Enemy eh = col.GetComponent<Enemy>();
                if (eh != null && eh.HasSpawned)
                    eh.TakeDamage(aoeDamage);
            }
        }

        // Optional visual feedback (e.g. flash or particle). Here, a quick debug circle:
        StartCoroutine(ShowAoeFlash(aoeDamageRadius, Color.red, 0.2f));
    }

    private IEnumerator ShowAoeFlash(float radius, Color color, float duration)
    {
        // Simple gizmo‐like flash: draw a circle for "duration" seconds
        float startTime = Runner.LocalRenderTime;
        while (Runner.LocalRenderTime < startTime + duration)
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.right * radius, color);
            Debug.DrawLine(transform.position, transform.position + Vector3.up * radius, color);
            yield return null;
        }
    }

    private void TryUseAbilityC()
    {
        if (!cAbilityCooldown.ExpiredOrNotRunning(Runner))
        {
            Debug.Log("Ability C is in cooldown");
            return;
        }
        Debug.Log("Ability C used: Icing (Slow)");
        IcingSlow();
        cAbilityCooldown = TickTimer.CreateFromSeconds(Runner, slowCooldown);
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
    
    private void OnColorChanged()
    {
        _spriteRenderer.color = SpriteColor;
    }

}


