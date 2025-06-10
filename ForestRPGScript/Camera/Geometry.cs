using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class Geometry : MonoBehaviour
{
    private Transform trans = null;
    private Vector3 rotate;

    public Transform SetParents { set { trans.SetParent(value); } }      //�θ� Ÿ�� ����
    public void Initialize()
    {
        rotate = Vector3.zero;
        trans = GetComponent<Transform>();
    }

    /// <summary>
    /// pitch : xȸ����
    /// yaw : yȸ����
    /// </summary>
    public void SetRotate(float pitch, float yaw)
    {
        rotate.x = pitch;
        rotate.y = yaw;
        trans.rotation = Quaternion.Euler(rotate);
    }   //ȸ���� ���� �� ȸ��
}
