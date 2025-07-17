using System;
using UnityEngine;

public abstract class ScriptableItemBase : ScriptableObject
{
    public int id;
    public string title;

    public PlayerInventory.PlayerClass itemClass;
    public string description;

    public int maxStackSize;
    public int tier;

    public Sprite sprite;

    public int price;

    public ScriptableActionBase action;


    public override string ToString()
    {
        return title + "is a " + description;
    }
}
