using UnityEngine;

public class Geometry : MonoBehaviour
{
    [SerializeField] Transform trans;

    /// <summary>
    /// rotatePitch : x�� ȸ��
    /// rotateYaw : y�� ȸ��
    /// </summary>
    public void SetRotate(float rotatePitch, float rotateYaw)
    {
        trans.rotation = Quaternion.Euler(rotatePitch, rotateYaw, 0.0f);       //������Ʈ�� x�� ȸ��
    }   //������Ʈ�� ȸ��
    public Transform GetTransform() { return this.gameObject.transform; }
}
