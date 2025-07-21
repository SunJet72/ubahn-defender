using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINicknamePanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputNickname;
    [SerializeField] Button confirmButton;
    [SerializeField] TMP_Text buttonText;

    private TouchScreenKeyboard keyboard;

    void Awake()
    {
        confirmButton.onClick.AddListener(() => GameFlowManager.instance.LogIn(inputNickname.text));
    }

    public void OnNonemptyNickname(string input)
    {
        if (input == "")
        {
            buttonText.text = "InputName";
            confirmButton.enabled = false;
        }
        else
        {
            buttonText.text = "Start";
            confirmButton.enabled = true;
        }
    }

    public void KeybordOpen()
    {
        Debug.Log("Keyboard");
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        keyboard.active = true;
    }

    public void KeybordClose()
    {
        //TouchScreenKeyboard.
    }
    
}
