using UnityEngine;

public class EquipItem : MonoBehaviour
{
    [SerializeField] private EPlayerEquipParts meEquipParts;        //����
    [SerializeField] Transform mPivot;      //�Ǻ�

    public Transform GetPivot => mPivot;
    public EPlayerEquipParts GetEquipParts => meEquipParts;

    public void SetActiveStatu(bool bActiveStatu)
    {
        this.gameObject.SetActive(bActiveStatu);
    }   //������Ʈ Ȱ��ȭ ����
}
