using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpellPreparationVisual : MonoBehaviour
{
    [SerializeField] private GameObject castedBackground;
    private SpellData spellData;
    private GameObject castedVisual;
    public void StartSpellPreparation(Spell spell, SpellButton spellButton)
    {
        spellData = spell.SpellData;
        spellButton.OnHandleMove += ManipulateCastedVisual;

        castedBackground.transform.localScale = new Vector3(2 * spellData.castRadius, 2 * spellData.castRadius);
        castedBackground.SetActive(true);

        if (spellData._castedVisualPrefab != null)
        {
            castedVisual = Instantiate(spellData._castedVisualPrefab);
            castedVisual.transform.parent = transform;
            castedVisual.transform.position = transform.position;
        }
    }

    private void ManipulateCastedVisual(Vector2 normilizedPosition)
    {
        if (castedVisual != null)
        {
            switch (spellData.castType)
            {
                case CastType.DIRECTION:
                    float angle = Mathf.Atan2(normilizedPosition.y, normilizedPosition.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90f);
                    castedVisual.transform.localRotation = rotation;
                    break;
                case CastType.POSITION:
                    castedVisual.transform.localPosition = normilizedPosition * spellData.castRadius;
                    break;
                case CastType.STATIC:
                    break;
            }
        }
    }

    public void EndSpellPreparation(Spell spell, SpellButton spellButton)
    {
        castedBackground.SetActive(false);
        spellButton.OnHandleMove -= ManipulateCastedVisual;
        spellData = null;
        Destroy(castedVisual);
        castedVisual = null;
    }
}
