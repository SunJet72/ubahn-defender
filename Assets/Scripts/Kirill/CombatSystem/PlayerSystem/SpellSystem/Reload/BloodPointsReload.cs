using UnityEngine;

public class BloodPointsReload : Reload
{
    [SerializeField] private BloodPointsReloadData data;
    [SerializeField] private BloodthirsterLabel btLabel;
    public override float ReloadPercentage => reloadPercentage;
    private float reloadPercentage;
    private bool hasEnoughPoints;
    private bool hasEnoughTimeSpent;
    private int curPoints = 0;

    private float timeSpent = 0f;

    public override void Spawned()
    {
        btLabel.OnBPChanged += SetBloodPoints;
        reloadPercentage = 0f;
        hasEnoughPoints = false;
    }

    public override bool IsReady()
    {
        return hasEnoughPoints && hasEnoughTimeSpent;
    }

    public override void FixedUpdateNetwork()
    {
        if (!hasEnoughTimeSpent)
        {
            timeSpent += Runner.DeltaTime;
            if (timeSpent >= data.minimalReloadTime)
            {
                timeSpent = data.minimalReloadTime;
                reloadPercentage = Mathf.Min(1f, (float)curPoints / data.bloodPointsCost);;
                hasEnoughTimeSpent = true;
                if (hasEnoughPoints && hasEnoughTimeSpent)
                    Trigger();
            }
            else
            {
                reloadPercentage = Mathf.Min(timeSpent / data.minimalReloadTime, (float)curPoints / data.bloodPointsCost);
            }
        }
    }

    private void SetBloodPoints(int points) // Increments points amount
    {
        if (!hasEnoughPoints)
        {
            curPoints++;
            if (curPoints >= data.bloodPointsCost)
            {
                curPoints = data.bloodPointsCost;
                reloadPercentage = Mathf.Min(timeSpent / data.minimalReloadTime, 1f);
                hasEnoughPoints = true;
                if (hasEnoughPoints && hasEnoughTimeSpent)
                    Trigger();
            }
            else
            {
                reloadPercentage = Mathf.Min(timeSpent / data.minimalReloadTime, (float)curPoints / data.bloodPointsCost);
            }
        }
    }

    public override void SpellWasUsed()
    {
        curPoints = 0;
        reloadPercentage = 0f;
        hasEnoughPoints = false;
    }
}
