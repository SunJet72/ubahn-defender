using System.Text;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAllItems : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StringBuilder str = new StringBuilder();
        List<ScriptableItemBase> allItems = ItemManager.instance.GetAll();
        str.Append("All items in the Game are:\n");
        foreach (ScriptableItemBase item in allItems)
        {
            str.Append(item.ToString()).Append("\n");
        }

        GetComponent<TMP_Text>().text = str.ToString();
    }

}
