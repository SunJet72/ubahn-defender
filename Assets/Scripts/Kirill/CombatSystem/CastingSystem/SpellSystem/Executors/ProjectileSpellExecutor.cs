using System.Collections;
using Fusion;
using UnityEngine;

public class ProjectileSpellExecutor : NetworkBehaviour
{
    private ProjectileSpellData data;
    private Transform castTransform;
    private PlayerCombatSystem player;

    [Networked]
    private TickTimer spellTimer { get; set; }

    public void Initialize(ProjectileSpellData spellData, Transform castTransform, Vector2 castedPoint, PlayerCombatSystem player)
    {
        data = spellData;
        this.player = player;
        this.castTransform = castTransform;
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

    public void Initialize(ProjectileSpellData spellData, Transform castTransform, Transform targetTransform, PlayerCombatSystem player)
    {
        data = spellData;
        this.player = player;
        this.castTransform = castTransform;
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
        if (!Runner.IsServer) return;
        if (!spellTimer.Expired(Runner)) return;

        float interval = data.executionTime / data.executionAmount;

        if (i < data.executionAmount)
        {
            SpawnProjectile(castedPointOrDirection);
            spellTimer = TickTimer.CreateFromSeconds(Runner, interval);
            i++;
        }
        else Runner.Despawn(Object);
    }
    private void ExecuteSpell(Transform targetTransform)
    {
        if (!Runner.IsServer) return;
        if (!spellTimer.Expired(Runner)) return;

        float interval = data.executionTime / data.executionAmount;

        if (i < data.executionAmount)
        {
            SpawnProjectile(targetTransform);
            spellTimer = TickTimer.CreateFromSeconds(Runner, interval);
            i++;
        }
        else Runner.Despawn(Object);
    }

    private void SpawnProjectile(Vector2 castedPointOrDirection)
    {
        Runner.Spawn(data._projectile, onBeforeSpawned: (runner, spawned) =>
        {
            spawned.transform.parent = null;
            spawned.transform.position = castTransform.position;
            spawned.GetComponent<Projectile>().SetTarget(castedPointOrDirection, player, data.damageProExecution, data.targetTypes);
        });
    }
    private void SpawnProjectile(Transform targetTransform)
    {
        Runner.Spawn(data._projectile, onBeforeSpawned: (runner, spawned) =>
        {
            spawned.transform.parent = null;
            spawned.transform.position = castTransform.position;
            spawned.GetComponent<Projectile>().SetTarget(targetTransform, player, data.damageProExecution, data.targetTypes);
        });
       
    }
}
