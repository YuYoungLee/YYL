using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class Geometry : MonoBehaviour
{
    private Transform trans = null;
    private Vector3 rotate;

    public Transform SetParents { set { trans.SetParent(value); } }      //부모 타겟 설정
    public void Initialize()
    {
        rotate = Vector3.zero;
        trans = GetComponent<Transform>();
    }

    /// <summary>
    /// pitch : x회전값
    /// yaw : y회전값
    /// </summary>
    public void SetRotate(float pitch, float yaw)
    {
        rotate.x = pitch;
        rotate.y = yaw;
        trans.rotation = Quaternion.Euler(rotate);
    }   //회전값 저장 후 회전
}
