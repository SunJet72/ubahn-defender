using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIEvents : MonoBehaviour
{
    public static event Action<int, int> OnHealthChanged;
    public static event Action<int, int> OnShieldChanged;
    public static event Action<int> OnPrismCountChanged;
    public static event Action<int, int> OnWaveProgressChanged;

    public static void HealthChanged(int current, int max)
    {
        OnHealthChanged?.Invoke(current, max);
    }

    public static void ShieldChanged(int current, int max)
    {
        OnShieldChanged?.Invoke(current, max);
    }

    public static void PrismCountChanged(int count)
    {
        OnPrismCountChanged?.Invoke(count);
    }

    public static void WaveProgressChanged(int currentWave, int totalWaves)
    {
        OnWaveProgressChanged?.Invoke(currentWave, totalWaves);
    }
}
