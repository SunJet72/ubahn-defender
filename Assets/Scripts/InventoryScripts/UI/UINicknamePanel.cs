using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINicknamePanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputNickname;
    [SerializeField] Button confirmButton;
    [SerializeField] TMP_Text buttonText;

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
}
