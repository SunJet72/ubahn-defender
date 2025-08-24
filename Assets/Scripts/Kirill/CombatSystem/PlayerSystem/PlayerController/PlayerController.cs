using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private float speed = 5f;
    private PlayerControls controls;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerCombatSystem playerCombatSystem;
    [SerializeField] private SpellPreparationVisual spellPreparationVisual;

    public float MoveInput
    { get; private set; }

    public override void Spawned()
    {
        Debug.LogWarning("Player COntroller have spawned");
        if (HasInputAuthority)
        {
            Debug.Log("Player COntroller has authority");
            if (controls == null)
                CreateControls();
            SetPlayerControls();
        }
    }
    // public void AfterSpawned()
    // {
    //     Debug.LogWarning("Player COntroller have spawned");
    //     if (HasInputAuthority)
    //     {
    //         Debug.Log("Player COntroller has authority");
    //         if (controls == null)
    //             CreateControls();
    //         SetPlayerControls();
    //     }
    // }

    private void SetPlayerControls()
    {
        playerCombatSystem.SetupPlayerController(this);
    }
    private void OnEnable()
    {
        if (controls == null)
            CreateControls();
    }

    private void CreateControls()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>().y;
        controls.Player.Move.canceled += ctx => MoveInput = 0f;
        controls.Enable();
    }
    public void MoveAs(float movementInc)
    {
        MoveInput += movementInc;
        if (MoveInput > 1f)
            MoveInput = 1f;
        if (MoveInput < -1f)
            MoveInput = -1f;
        SetCharacterSprite();
    }
    private void SetCharacterSprite()
    {
        if (MoveInput >= 0f)
            playerCombatSystem.SetCharacterSprite(false);
        else
            playerCombatSystem.SetCharacterSprite(true);
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<NetworkInputData>(out var input) == false) return;
        rb.linearVelocityY = input.moveInput * playerCombatSystem.Speed;
    }

    public void StartSpellPreparation(Spell spell, SpellButton spellButton)
    {
        spellPreparationVisual.StartSpellPreparation(spell, spellButton);
    }
    
    public void EndSpellPreparation(Spell spell, SpellButton spellButton)
    {
        spellPreparationVisual.EndSpellPreparation(spell, spellButton);
    }
}
