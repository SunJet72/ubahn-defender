using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileData data;
    Transform target;
    Vector2 flyingDirection;
    bool isFlying = false;
    public void SetTarget(Transform target)
    {
        this.target = target;
        UpdateFlyingDirection();

        isFlying = true;
    }

    void FixedUpdate()
    {
        if (isFlying)
        {
            if (data.isSelfGuided)
            {
                UpdateFlyingDirection();
            }
            transform.up = new Vector3(flyingDirection.x, flyingDirection.y, 0);
            transform.Translate(Vector2.up * data.speed * Time.fixedDeltaTime);
        }
    }

    void UpdateFlyingDirection()
    {
        flyingDirection = (target.position - transform.position).normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
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
