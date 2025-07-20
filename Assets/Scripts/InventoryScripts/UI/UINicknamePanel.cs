using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINicknamePanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputNickname;
    [SerializeField] Button confirmButton;

    void Awake()
    {
        confirmButton.onClick.AddListener(() => GameFlowManager.instance.LogIn(inputNickname.text));
    }
}
