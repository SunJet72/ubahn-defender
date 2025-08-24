using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
    [Header("Health UI")]
    [Tooltip("Slider representing health bar")]
    [SerializeField] private Slider healthSlider;
    [Tooltip("Text on top of health bar to show numeric values")]
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Shield UI")]
    [Tooltip("Slider representing shield bar")]
    [SerializeField] private Slider shieldSlider;
    [Tooltip("Text on top of shield bar to show numeric values")]
    [SerializeField] private TextMeshProUGUI shieldText;

    [Header("Prism UI")]
    [SerializeField] private TextMeshProUGUI prismCountText;

    [Header("Wave UI")]
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Spell Buttons")]
    [SerializeField] private SpellButton armorSpellButton;
    [SerializeField] private SpellButton weaponSpellButton;
    [Header("Walking Buttons")]
    [SerializeField] private Button walkingUpButton;
    [SerializeField] private Button walkingDownButton;

    private PlayerController playerController;

    private void OnEnable()
    {
        UIEvents.OnHealthChanged += UpdateHealth;
        UIEvents.OnShieldChanged += UpdateShield;
        UIEvents.OnPrismCountChanged += UpdatePrismCount;
        UIEvents.OnWaveProgressChanged += UpdateWaveProgress;
    }

    private void OnDisable()
    {
        UIEvents.OnHealthChanged -= UpdateHealth;
        UIEvents.OnShieldChanged -= UpdateShield;
        UIEvents.OnPrismCountChanged -= UpdatePrismCount;
        UIEvents.OnWaveProgressChanged -= UpdateWaveProgress;
    }

    private void UpdateHealth(int current, int max)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }
        if (healthText != null)
            healthText.text = $"{current}/{max}";
    }

    private void UpdateShield(int current, int max)
    {
        if (shieldSlider != null)
        {
            shieldSlider.maxValue = max;
            shieldSlider.value = current;
        }
        if (shieldText != null)
            shieldText.text = $"{current}/{max}";
    }

    private void UpdatePrismCount(int count)
    {
        if (prismCountText != null)
            prismCountText.text = $"x{count}";
    }

    private void UpdateWaveProgress(int currentWave, int totalWaves)
    {
        if (waveText != null)
            waveText.text = $"Wave {currentWave}/{totalWaves}";
    }

    public void SetSpells(PlayerCombatSystem player, Spell spellArmor, Spell spellWeapon)
    {
        armorSpellButton.SetSpell(player, spellArmor);
        weaponSpellButton.SetSpell(player, spellWeapon);
    }
    public void SetupPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
        walkingUpButton.onClick.AddListener(MoveUp);
        walkingDownButton.onClick.AddListener(MoveDown);
    }
    public void StartSpellNavigation(Spell spell, SpellButton spellButton)
    {
        playerController.StartSpellPreparation(spell, spellButton);
    }

    public void EndSpellNavigation(Spell spell, SpellButton spellButton)
    {
        playerController.EndSpellPreparation(spell, spellButton);
    }
    public void MoveUp() => playerController.MoveAs(1f);
    public void MoveDown() => playerController.MoveAs(-1f);
}
