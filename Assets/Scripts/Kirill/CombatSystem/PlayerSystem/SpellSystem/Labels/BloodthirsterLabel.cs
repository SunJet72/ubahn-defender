using System;
using UnityEngine;

public class BloodthirsterLabel : Label
{
    private int bloodPoints;
    public int BloodPoints
    {
        get => bloodPoints;
        private set
        {
            bloodPoints = value;
            OnBPChanged?.Invoke(value);
        }
    }
    public int DirtPoints { get; private set; }
    public Action<int> OnBPChanged;
    protected override void InitAsArmor()
    {
        BloodPoints = 0;
        DirtPoints = 0;
        player.OnHurtEvent += IncreaseBloodPoints;
    }

    protected override void InitAsWeapon()
    {
        BloodPoints = 0;
        DirtPoints = 0;
        player.OnHurtTargetEvent += IncreaseBloodPoints;
        (weapon.GetSpell() as ActiveSpell).OnSpellActivated += ResetBloodPoints;
    }

    private void IncreaseBloodPoints(UnitController attacker, UnitController target) {
        BloodPoints++;
        DirtPoints++;
    }

    private void ResetBloodPoints() {
        BloodPoints = 0;
    }
}
