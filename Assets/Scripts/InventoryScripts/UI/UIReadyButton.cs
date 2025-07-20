using UnityEngine;
using UnityEngine.UI;

public class UIReadyButton : MonoBehaviour
{
    Button button;

    [SerializeField] private Sprite readySprite;
    [SerializeField] private Sprite unReadySprite;

    Image image;

    bool ready = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        image.sprite = unReadySprite;
        button.onClick.AddListener(Toggle);
    }

    void Toggle()
    {
        if (ready)
        {
            ready = false;
            image.sprite = unReadySprite;
            GameFlowManager.instance.GetUnready();
        }
        else
        {
            ready = true;
            image.sprite = readySprite;
            GameFlowManager.instance.GetReady();
        }
    }
}
