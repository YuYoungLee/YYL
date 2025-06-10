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
        mMainCamera.SetActive(bActive);     //Ȱ��ȭ
    }   //���� ī�޶� Ȱ��ȭ ����

    /// <summary>
    /// VCCamera : Ȱ��ȭ �� VCī�޶�
    /// </summary>
    public void StatrtChangeVCCamera(GameObject VCCamera)
    {
        VCCamera.SetActive(true);
        mPlayerVCCamera.SetActive(false);
    }   //VCī�޶� ���� Ȱ��ȭ

    /// <summary>
    /// VCCamera : ��Ȱ��ȭ �� VCī�޶�
    /// </summary>
    public void EndChangeVCCamera(GameObject VCCamera)
    {
        mPlayerVCCamera.SetActive(true);
        VCCamera.SetActive(false);
    }   //VCī�޶� ���� ��Ȱ��ȭ
}
