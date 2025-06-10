using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera mCamera;
    private Transform mTransform;

    [SerializeField] private GameObject mMainCamera;
    [SerializeField] private GameObject mPlayerVCCamera;

    public Vector3 GetPos => mTransform.position;

    public Transform Transform => mTransform;

    public void Initialize()
    {
        if(mCamera == null)
        {
            mCamera = GetComponent<Camera>();
        }
        
        if(mTransform == null)
        {
            mTransform = GetComponent<Transform>();
        }
    }

    public void ActiveMainCamera(bool bActive)
    {
        mMainCamera.SetActive(bActive);     //활성화
    }   //메인 카메라 활성화 변경

    /// <summary>
    /// VCCamera : 활성화 할 VC카메라
    /// </summary>
    public void StatrtChangeVCCamera(GameObject VCCamera)
    {
        VCCamera.SetActive(true);
        mPlayerVCCamera.SetActive(false);
    }   //VC카메라 변경 활성화

    /// <summary>
    /// VCCamera : 비활성화 할 VC카메라
    /// </summary>
    public void EndChangeVCCamera(GameObject VCCamera)
    {
        mPlayerVCCamera.SetActive(true);
        VCCamera.SetActive(false);
    }   //VC카메라 변경 비활성화
}
