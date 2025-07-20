using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public ScriptableItemBase emptyItem;
    public ScriptableArmor emptyArmor;
    public ScriptableWeapon emptyWeapon;
    public Sprite defaultSprite;
    private Dictionary<int, ScriptableItemBase> allItems;
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

        // To reset ids of all items. Do with caution
        //ScrambleIds(resources);
        allItems = resources.ToDictionary(i => i.id);
        DontDestroyOnLoad(this);
    }

    public ScriptableItemBase getItem(int id)
    {
        return allItems.TryGetValue(id, out ScriptableItemBase item) ? item : null;
    }

    public ScriptableItemBase getItem(string title)
    {
        return allItems.FirstOrDefault(kvp => kvp.Value.title == title).Value;
    }

    public List<ScriptableItemBase> GetAll()
    {
        return allItems.Values.ToList<ScriptableItemBase>();
    }

    public List<ScriptableItemBase> GetAll<T>() where T : ScriptableItemBase
    {
        return allItems.Values.OfType<T>().ToList<ScriptableItemBase>();
    }

    public List<ScriptableItemBase> GetAll(int tier)
    {
        return allItems.Values.Where(item => item.tier == tier).ToList();
    }

    public void ScrambleIds(ScriptableItemBase[] items)
    {
        int counter = 1;
        foreach (ScriptableItemBase item in items)
        {
            item.id = counter++;
        }
    }

}
