using System.Collections;
using Fusion;
using UnityEngine;

public class ProjectileSpellExecutor : NetworkBehaviour
{
    private ProjectileSpellData data;
    private Transform castTransform;

    [Networked]
    private TickTimer spellTimer { get; set; }

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
            spellTimer = TickTimer.CreateFromSeconds(Runner, data.executionDelay);
            _castedPoint = direction;
            isTransform = false;
            ExecuteSpell(direction);
        }
        else
        {
            spellTimer = TickTimer.CreateFromSeconds(Runner, data.executionDelay);
            _castedPoint = castedPoint;
            isTransform = false;
            ExecuteSpell(castedPoint);
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
        spellTimer = TickTimer.CreateFromSeconds(Runner, data.executionDelay);
        _targetTransform = targetTransform;
        isTransform = true;
        ExecuteSpell(targetTransform);
    }

    private Transform _targetTransform;
    private Vector2 _castedPoint;
    private bool isTransform;


    public override void FixedUpdateNetwork()
    {
        if (spellTimer.Expired(Runner))
        {
            if (isTransform) ExecuteSpell(_targetTransform);
            else ExecuteSpell(_castedPoint);
        }
    }

    private int i = 0;

    private void ExecuteSpell(Vector2 castedPointOrDirection)
    {
        if (!spellTimer.Expired(Runner)) return;

        float interval = data.executionTime / data.executionAmount;

        if (i < data.executionAmount)
        {
            SpawnProjectile(castedPointOrDirection);
            spellTimer = TickTimer.CreateFromSeconds(Runner, interval);
            i++;
        }
        else Destroy(this);
    }
    private void ExecuteSpell(Transform targetTransform)
    {
        if (!spellTimer.Expired(Runner)) return;

        float interval = data.executionTime / data.executionAmount;

        if (i < data.executionAmount)
        {
            SpawnProjectile(targetTransform);
            spellTimer = TickTimer.CreateFromSeconds(Runner, interval);
            i++;
        }
        else Destroy(this);
    }

    private void SpawnProjectile(Vector2 castedPointOrDirection)
    {
        NetworkObject projectile = Runner.Spawn(data._projectile);
        projectile.transform.position = transform.position;
        projectile.GetComponent<Projectile>().SetTarget(castedPointOrDirection);
    }
    private void SpawnProjectile(Transform targetTransform)
    {
        NetworkObject projectile = Runner.Spawn(data._projectile);
        projectile.transform.position = transform.position;
        projectile.GetComponent<Projectile>().SetTarget(targetTransform);
    }
}
