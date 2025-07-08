using TMPro;
using UnityEngine;

public class UIInventoryText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]private GameObject player;
    private PlayerInventory inventory;
    private TMP_Text text;
    void Awake()
    {
        player = GameObject.Find("Player");
        text = gameObject.GetComponent<TMP_Text>();
        inventory = player.GetComponent<PlayerInventory>();
        inventory.InventoryChanged.AddListener(ChangeText);

    }

    void OnEnable()
    {
        inventory.InventoryChanged.AddListener(ChangeText);
    }

    void Oisable()
    {
        inventory.InventoryChanged.RemoveListener(ChangeText);        
   }

    public void ChangeText()
    {
        text.text = inventory.ToString();
    }
}
