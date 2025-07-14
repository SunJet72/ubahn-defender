using UnityEngine;

public class FingAbomination : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject suspect;
    void Start()
    {
        suspect = WorldMapController.instance.gameObject;
    }
}
