using Cinemachine;
using UnityEngine;

public class ThirdPointCamera : MonoBehaviour
{
    private Transform trans = null;
    private CinemachineVirtualCamera vCamera = null;
    public void Initialize()
    {
        trans = GetComponent<Transform>();
        vCamera = GetComponent<CinemachineVirtualCamera>();
    }
}
