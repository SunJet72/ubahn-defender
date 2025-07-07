using UnityEngine;
using UnityEngine.Events;

public class ExampleAction : MonoBehaviour, IActionable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created\
    private SpriteRenderer playerRenderer;

    public bool state = true;
    private Color startColor;
    [SerializeField]private Color abilityColor;

    public void SetUp(GameObject player, ScriptableActionBase action, UnityEvent actionEvent)
    {
        playerRenderer = player.GetComponent<SpriteRenderer>();
        startColor = playerRenderer.color;

        if (action is ActionChangeColor)
        {
            abilityColor = ((ActionChangeColor)action).color;
        }
        else
        {
            Debug.LogError("The Fuck wrong with you. Wrong Ability, no Color");
        }

        actionEvent.AddListener(Action);

        }
    public void Action()
    {
        if (state)
        {
            playerRenderer.color = abilityColor;
            state = !state;
        }
        else
        {
            playerRenderer.color = startColor;
            state = !state;
        }
    }
}
