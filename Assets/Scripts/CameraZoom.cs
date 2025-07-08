using Unity.Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineCamera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        camera.Lens.OrthographicSize -= Input.GetAxis("Mouse ScrollWheel");
    }
}
