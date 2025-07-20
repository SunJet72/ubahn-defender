using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [SerializeField]private List<ScriptableItemBase> objects;
    [SerializeField] private ScriptableItemBase sample;

    public int Count
    {
        get => objects.Count;
    }

    public InventorySlot(ScriptableItemBase sample)
    {
        this.sample = sample;
        objects = new List<ScriptableItemBase>();
    }

    public InventorySlot()
    {
        objects = new List<ScriptableItemBase>();
    }

    public InventorySlot AddItem(ScriptableItemBase item)
    {
        if (item.name == sample.name)
        {
            objects.Add(item);
        }
        else
        {
            Debug.LogError("Item in wrong inventory slot");
        }
        return this;
    }

    public InventorySlot RemoveItem()
    {
        if (objects.Count != 0)
        {
            objects.RemoveAt(0);
        }
        else
        {
            Debug.LogError("Removing nonexistent item, " + sample.name);
        }
        return this;
    }

    public ScriptableItemBase GetSample()
    {
        return sample;
    }

    public override string ToString()
    {
        return Count + " of " + sample;
    }
}
