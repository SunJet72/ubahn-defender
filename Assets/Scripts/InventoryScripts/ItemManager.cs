using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class ItemManager: MonoBehaviour
{
    public static ItemManager instance;
    public ScriptableItemBase emptyItem;
    public ScriptableArmor emptyArmor;
    public ScriptableWeapon emptyWeapon;
    public Sprite defaultSprite;
    private Dictionary<string, ScriptableItemBase> allItems;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        emptyItem = ScriptableObject.CreateInstance<EmptyItem>();
        emptyItem.name = "Empty";
        emptyArmor = ScriptableObject.CreateInstance<ScriptableArmor>();
        emptyArmor.name = "Empty";
        emptyWeapon = ScriptableObject.CreateInstance<ScriptableWeapon>();
        emptyWeapon.name = "Empty";

        defaultSprite = Resources.Load<Sprite>("Sprites/DefaultSprite");
        var resources = Resources.LoadAll<ScriptableItemBase>("AllItems");
        allItems = resources.ToDictionary(i => i.title);
        DontDestroyOnLoad(this);
    }

    public ScriptableItemBase getItem(string title)
    {
        return allItems.TryGetValue(title, out ScriptableItemBase item) ? item : null;
    }

    public List<ScriptableItemBase> GetAll()
    {
        return allItems.Values.ToList<ScriptableItemBase>();
    }

    public List<ScriptableItemBase> GetAll<T>() where T : ScriptableItemBase
    {
        return allItems.Values.OfType<T>().ToList<ScriptableItemBase>();
    }
}
