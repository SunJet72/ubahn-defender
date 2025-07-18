using System;
using UnityEngine;

public abstract class ScriptableItemBase : ScriptableObject
{
    public string title;
    public string description;

    public int maxStackSize;

    public Sprite sprite;

    public ScriptableActionBase action;
    
    public GameObject spell;


    public override string ToString()
    {
        return title + "is a " + description;
    }
}
