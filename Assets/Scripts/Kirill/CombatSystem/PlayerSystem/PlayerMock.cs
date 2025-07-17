using System;
using Fusion;
using UnityEngine;

public class PlayerMock : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnColorChanged))] public Color SpriteColor { get; set; }
    public EventHandler onDieEvent;
    [Networked]
    public float Health { get; set; }

    public SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Hurt(float damage)
    {
        Health -= damage;
        Debug.Log("Player got hurt. Current health: " + Health);
        if (Health <= 0)
        {
            Health = 0;
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

      private void OnColorChanged()
    {
        _spriteRenderer.color = SpriteColor;
    }

}
