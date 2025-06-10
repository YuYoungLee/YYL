using UnityEngine;

public class Geometry : MonoBehaviour
{
    [SerializeField] Transform trans;

    /// <summary>
    /// rotatePitch : x축 회전
    /// rotateYaw : y축 회전
    /// </summary>
    public void SetRotate(float rotatePitch, float rotateYaw)
    {
        trans.rotation = Quaternion.Euler(rotatePitch, rotateYaw, 0.0f);       //지오메트리 x축 회전
    }   //지오메트리 회전
    public Transform GetTransform() { return this.gameObject.transform; }
}
