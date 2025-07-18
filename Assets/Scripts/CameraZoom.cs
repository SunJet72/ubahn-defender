using Unity.Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineCamera cinCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cinCamera = GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        cinCamera.Lens.OrthographicSize -= Input.GetAxis("Mouse ScrollWheel");
    }
}
