using System.Collections;
using UnityEngine;

public class SplashSpellExecutor : MonoBehaviour
{
    private SplashSpellData data;
    private Transform castTransform;
    private Vector2 direction;

    public void Initialize(SplashSpellData spellData, Transform castTransform, Vector2 castedPoint)
    {
        data = spellData;
        this.castTransform = castTransform;
        direction = castedPoint - (Vector2)castTransform.position;
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
        Collider[] hits = Physics.OverlapSphere(castTransform.position, data.radius);
        foreach (var hit in hits)
        {
            Vector3 toTarget = (hit.transform.position - castTransform.position);
            toTarget.y = 0;

            float angle = Vector3.Angle(direction, toTarget.normalized);
            if (angle <= data.fov / 2f)
            {
                Debug.Log("I hit the enemy with splash: " + hit.gameObject);
                /*if (hit.TryGetComponent(out IEnemy enemy))
                {
                    enemy.TakeDamage(data.damageProExecution);
                }*/
            }
        }
    }
}
