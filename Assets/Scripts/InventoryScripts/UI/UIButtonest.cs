using Unity.VisualScripting;
using UnityEngine;

public class UIButtonTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject player;
    private PlayerInventory inventory;

    public ScriptableItemBase item;
    void Awake()
    {
        player = GameObject.Find("Player");
        inventory = player.GetComponent<PlayerInventory>();
    }


}
