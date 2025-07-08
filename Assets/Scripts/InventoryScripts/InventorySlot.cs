using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class InventorySlot
{
    private List<ScriptableItemBase> objects;
    public ScriptableItemBase Sample { get; }

    public int Count
    {
        get => objects.Count;
    }

    public InventorySlot(ScriptableItemBase sample)
    {
        Sample = sample;
        objects = new List<ScriptableItemBase>();
    }

    public InventorySlot AddItem(ScriptableItemBase item)
    {
        if (item.name == Sample.name)
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
            Debug.LogError("Removing nonexistent item, " + Sample.name);
        }
        return this;
    }

    public override string ToString()
    {
        return Count + " of " + Sample;
    }
}
