using Fusion;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : NetworkBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPreparingSpell = false;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private ActiveSpell spell;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private Image icon;
    private PlayerMock playerMock;


    public void OnPointerDown(PointerEventData eventData)
    {
        isPreparingSpell = true;
        Debug.Log("Spell is being prepared...");
        // TODO: Visual Adjustment of spell preparation
    }

    public override void Spawned()
    {
        this.playerMock = NetworkManager.Instance.GetCurrentPlayer();
        spell = NetworkManager.Instance.GetCurrentPlayerActiveSpells()[0]; // TODO: this is Mock
        icon.sprite = spell.SpellData.icon;
    }

    void FixedUpdate()
    {
        if (spell == null) return;
        cooldownOverlay.fillAmount = 1f - spell.Reload.ReloadPercentage;
        Debug.Log(spell.Reload.ReloadPercentage);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPreparingSpell) return;

        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y));

        spell.Activate(playerMock, playerMock.gameObject.transform, worldMousePosition);

        Debug.Log("Spell cast toward: " + worldMousePosition);

        isPreparingSpell = false;
    }
}
