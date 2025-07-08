using System.Collections;
using UnityEngine;

public class CircleSpellExecutor : MonoBehaviour
{
    private CircleSpellData data;
    private Transform castTransform;

    public void Initialize(CircleSpellData spellData, Transform castTransform)
    {
        data = spellData;
        this.castTransform = castTransform;
        StartCoroutine(ExecuteSpell());
    }

    private IEnumerator ExecuteSpell()
    {
        yield return new WaitForSeconds(data.executionDelay);

        float interval = data.executionTime / data.executionAmount;

        for (int i = 0; i < data.executionAmount; i++)
        {
            DealDamage();
            yield return new WaitForSeconds(interval);
        }

        Destroy(this);
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
