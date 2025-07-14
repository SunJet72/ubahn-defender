using System;
using UnityEngine;

public class TimeReload : Reload
{
    [SerializeField] private TimeReloadData data;
    public override float ReloadPercentage => reloadPercentage;
    private float reloadPercentage;
    private bool isReady;
    private float timeSpent;
    void Awake()
    {
        reloadPercentage = 1f;
        isReady = true;
    }

    void FixedUpdate()
    {
        if (!isReady)
        {
            timeSpent += Time.fixedDeltaTime;
            if (timeSpent >= data.reloadTime)
            {
                timeSpent = 0;
                reloadPercentage = 1f;
                isReady = true;
                Trigger();
            }
            else
            {
                reloadPercentage = timeSpent / data.reloadTime;
            }
        }
    }

    public override void SpellWasUsed()
    {
        timeSpent = 0;
        reloadPercentage = 0;
        isReady = false;
    }
    public override bool IsReady()
    {
        return isReady;
    }
}
