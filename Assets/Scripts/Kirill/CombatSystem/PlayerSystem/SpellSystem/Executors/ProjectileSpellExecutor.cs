using System.Collections;
using UnityEngine;

public class ProjectileSpellExecutor : MonoBehaviour
{
    private ProjectileSpellData data;
    private Transform castTransform;

    public void Initialize(ProjectileSpellData spellData, Transform castTransform, Vector2 castedPoint)
    {
        data = spellData;
        if (data.targetType == TargetType.CURRENT_TARGET)
        {
            Debug.LogError("Projectile has wrong target type, or the wrong function was called");
            return;
        }
        if (data.targetType == TargetType.DIRECTION)
        {
            Vector2 direction = castedPoint - (Vector2)castTransform.position;
            StartCoroutine(ExecuteSpell(direction));
        }
        else
        {
            StartCoroutine(ExecuteSpell(castedPoint));
        }
        
    }

    public void Initialize(ProjectileSpellData spellData, Transform castTransform, Transform targetTransform)
    {
        data = spellData;
        if (data.targetType != TargetType.CURRENT_TARGET)
        {
            Debug.LogError("Projectile has wrong target type, or the wrong function was called");
            return;
        }
        StartCoroutine(ExecuteSpell(targetTransform));
    }

    private IEnumerator ExecuteSpell(Vector2 castedPointOrDirection)
    {
        yield return new WaitForSeconds(data.executionDelay);

        float interval = data.executionTime / data.executionAmount;

        for (int i = 0; i < data.executionAmount; i++)
        {
            SpawnProjectile(castedPointOrDirection);
            yield return new WaitForSeconds(interval);
        }

        Destroy(this);
    }
    private IEnumerator ExecuteSpell(Transform targetTransform)
    {
        yield return new WaitForSeconds(data.executionDelay);

        float interval = data.executionTime / data.executionAmount;

        for (int i = 0; i < data.executionAmount; i++)
        {
            SpawnProjectile(targetTransform);
            yield return new WaitForSeconds(interval);
        }

        Destroy(this);
    }

    private void SpawnProjectile(Vector2 castedPointOrDirection)
    {
        var projectile = Instantiate(data._projectile);
        projectile.transform.position = transform.position;
        projectile.GetComponent<Projectile>().SetTarget(castedPointOrDirection);
    }
    private void SpawnProjectile(Transform targetTransform)
    {
        var projectile = Instantiate(data._projectile);
        projectile.transform.position = transform.position;
        projectile.GetComponent<Projectile>().SetTarget(targetTransform);
    }
}
