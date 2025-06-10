using UnityEngine;
using Cinemachine;

public class ThirdPointCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera cineMachine;
    private Transform trCamera;
    private CinemachineVirtualCamera virCamera;


    public void Initialize()
    {
        cineMachine = GetComponent<CinemachineVirtualCamera>();
        trCamera = GetComponent<Transform>();
        virCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetTarget(Transform target)
    {
        cineMachine.Follow = target;
        cineMachine.LookAt = target;
    }

    public void SetPosition(Vector3 pos)
    {
        virCamera.PreviousStateIsValid = false;
        trCamera.position = pos;
    }

    public Vector3 GetPos() { return trCamera.position; }
}
