using System;
using Fusion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool isPreparingSpell = false;

    [SerializeField] private UIController ui;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private Image icon;
    [SerializeField] private Joystick joystick;
    private PlayerCombatSystem player;
    private SpellCaster spellCaster;
    private Spell spell;

    public event Action<Vector2> OnHandleMove;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPreparingSpell = true;
        Debug.Log("Spell is being prepared...");
        // TODO: Visual Adjustment of spell preparation

        joystick.gameObject.SetActive(true);

        joystick.OnPointerDown(eventData);

        EventSystem.current.SetSelectedGameObject(joystick.gameObject);

        ui.StartSpellNavigation(spell, this);
    }

    // public override void Spawned()
    // {
    //     
    //     spell = NetworkManager.Instance.GetCurrentPlayerActiveSpells()[0]; // TODO: this is Mock
    //     icon.sprite = spell.SpellData.icon;
    //     //icon.sprite = spell.SpellData.icon;
    // }

    public void SetSpell(PlayerCombatSystem player, SpellCaster spellCaster)
    {
        this.player = player;
        this.spellCaster = spellCaster;
        spell = spellCaster.GetSpell();
        
        icon.sprite = spell.SpellData.icon;

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
        //cooldownOverlay.fillAmount = 1f - spellCaster.ReloadPercentage; (TODO: OnReloadPercentageChanged)
    }

    public void OnDrag(PointerEventData eventData)
    {
        joystick.OnDrag(eventData);
        OnHandleMove?.Invoke(joystick.Direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPreparingSpell) return;

        /*Vector3 mouseScreenPosition = Input.mousePosition;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y));
        spell.Activate(player.Object, player.Object, worldMousePosition);*/

        Vector2 worldMousePosition = (Vector2)player.gameObject.transform.position + joystick.Direction * spell.SpellData.castRadius;
        //spell.Activate(player.Object, worldMousePosition); //TODO: Migrate all spell logic to uiCOntroller

        ui.EndSpellNavigation(spell, this);

        joystick.OnPointerUp(eventData);

        joystick.gameObject.SetActive(false);
        isPreparingSpell = false;
    }

    private void CastSpell()
    {
        // Cast Spell from SpellCaster
    }
}
