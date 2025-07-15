using UnityEngine;

public class PlayerController : MonoBehaviour
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

    void FixedUpdate()
    {
        rb.linearVelocityY = moveInput * speed;
    }
}
