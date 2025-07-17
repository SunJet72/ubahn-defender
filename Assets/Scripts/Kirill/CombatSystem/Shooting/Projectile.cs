using Fusion;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private ProjectileData data;
    Transform target;
    Vector2 flyingDirection;
    bool isFlying = false;
    public void SetTarget(Transform target)
    {
        if (data.targetType != TargetType.CURRENT_TARGET)
        {
            Debug.LogError("Projectile has wrong target type, or the wrong function was called");
            return;
        }
        this.target = target;
        UpdateFlyingDirection(target.position);

        isFlying = true;
    }

    public void SetTarget(Vector2 positionOrDirection)
    {
        if (data.targetType == TargetType.CURRENT_TARGET)
        {
            Debug.LogError("Projectile has wrong target type, or the wrong function was called");
            return;
        }
        if (data.targetType == TargetType.DIRECTION)
        {
            flyingDirection = positionOrDirection;
        }
        else
        {
            UpdateFlyingDirection(positionOrDirection);
        }

        isFlying = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (isFlying)
        {
            if (data.targetType == TargetType.CURRENT_TARGET)
            {
                UpdateFlyingDirection(target.position);
            }
            transform.up = new Vector3(flyingDirection.x, flyingDirection.y, 0);
            transform.Translate(Vector2.up * data.speed * Runner.DeltaTime);
        }
    }

    void UpdateFlyingDirection(Vector2 position)
    {
        flyingDirection = (position - (Vector2)transform.position).normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (data.hitType == HitType.NOBODY)
            return;
        collision.gameObject.TryGetComponent<PlayerMock>(out PlayerMock playerMock);
        if (playerMock != null)
        {
            playerMock.Hurt(data.damage);
            DestroyGO();
        }
    }

    private void DestroyGO()
    {
        Destroy(gameObject);
    }
}
