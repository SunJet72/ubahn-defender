using System;
using UnityEngine;

public class PlayerMock : MonoBehaviour
{
    public EventHandler onDieEvent;
    public float health;
    public void Hurt(float damage)
    {
        health -= damage;
        Debug.Log("Player got hurt. Current health: " + health);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player Died");
        onDieEvent.Invoke(this, null);
        Destroy(gameObject);
    }

    public Transform GetCurrentTargetSelected()
    {
        return transform; // Mock
    }
}
