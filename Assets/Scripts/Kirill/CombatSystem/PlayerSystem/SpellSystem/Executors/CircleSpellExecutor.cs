using System.Collections;
using Fusion;
using UnityEngine;

public class CircleSpellExecutor : NetworkBehaviour
{
    private CircleSpellData data;
    private Transform castTransform;

    [Networked]
    private TickTimer spellTimer { get; set; }

    public void Initialize(CircleSpellData spellData, Transform castTransform)
    {
        data = spellData;
        this.castTransform = castTransform;
        spellTimer = TickTimer.CreateFromSeconds(Runner, data.executionDelay);
    }
    private int i = 0;

    public override void FixedUpdateNetwork()
    {
        if (!spellTimer.Expired(Runner)) return;
        
        float interval = data.executionTime / data.executionAmount;

        if (i < data.executionAmount)
        {
            DealDamage();
            spellTimer = TickTimer.CreateFromSeconds(Runner, interval);
            i++;
        }
        else
        {
            Destroy(this);
        }
    }

    private void DealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(castTransform.position, data.radius, LayerMask.GetMask("Enemy"));

        foreach (Collider hit in hits)
        {
            Debug.Log("I hit an enemy with circle: " + hit.gameObject);
            /*var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(data.damageProExecution);
            }*/
        }
    }
}
