using Fusion;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPreparingSpell = false;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private Image icon;
    private PlayerCombatSystem player;

    private ActiveSpell spell;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPreparingSpell = true;
        Debug.Log("Spell is being prepared...");
        // TODO: Visual Adjustment of spell preparation
    }

    // public override void Spawned()
    // {
    //     
    //     spell = NetworkManager.Instance.GetCurrentPlayerActiveSpells()[0]; // TODO: this is Mock
    //     icon.sprite = spell.SpellData.icon;
    //     //icon.sprite = spell.SpellData.icon;
    // }

    public void SetSpell(PlayerCombatSystem player, Spell spell)
    {
        this.player = player;

        Debug.Log("I AM SETTING SPELL: " + spell);
        if (spell is ActiveSpell activeSpell)
        {
            this.spell = activeSpell;
            icon.sprite = activeSpell.SpellData.icon;
        }
    }

    void FixedUpdate()
    {
        if (spell == null) return;
        cooldownOverlay.fillAmount = 1f - spell.Reload.ReloadPercentage;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPreparingSpell) return;

        Debug.Log(spell);
        Debug.Log(player);

        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y));

        spell.Activate(player.Object, player.Object, worldMousePosition);

        Debug.Log("Spell cast toward: " + worldMousePosition);

        isPreparingSpell = false;
    }
}
