using UnityEngine;

public class EquipItem : MonoBehaviour
{
    [SerializeField] private EPlayerEquipParts meEquipParts;        //파츠
    [SerializeField] Transform mPivot;      //피봇

    public Transform GetPivot => mPivot;
    public EPlayerEquipParts GetEquipParts => meEquipParts;

    public void SetActiveStatu(bool bActiveStatu)
    {
        this.gameObject.SetActive(bActiveStatu);
    }   //오브젝트 활성화 변경
}
