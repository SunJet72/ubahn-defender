using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private float speed = 5f;
    private PlayerControls controls;
    [SerializeField] private Rigidbody2D rb;

    public float MoveInput { get; private set; }

    // public override void Spawned()
    // {
       
    // }
    private void OnEnable()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>().y;
        controls.Player.Move.canceled += ctx => MoveInput = 0f;
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<NetworkInputData>(out var input) == false) return;
        rb.linearVelocityY = input.moveInput * speed;
    }
}
