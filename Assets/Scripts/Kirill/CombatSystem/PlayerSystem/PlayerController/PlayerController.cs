using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private float speed = 5f;
    private PlayerControls controls;
    [SerializeField] private Rigidbody2D rb;
    private float moveInput;

    void Awake()
    {
        controls = new PlayerControls();
    }
    private void OnEnable()
    {
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>().y;
        controls.Player.Move.canceled += ctx => moveInput = 0f;
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    public override void FixedUpdateNetwork()
    {
        rb.linearVelocityY = moveInput * speed;
    }
}
