using System.Collections.Generic;
using UnityEngine;

public class EquipItemManager : MonoBehaviour
{
    [SerializeField] private List<Transform> mParentObj;     //아이템 부모 위치

    private Dictionary<string, EquipItem> mEquipItem;      //장착 아이템 오브젝트

    public void Initialize()
    {
        //아이템 딕셔너리 삽입
        if(mEquipItem == null)
        {
            mEquipItem = new Dictionary<string, EquipItem>();
            EquipItem equipItem;
            //부모 아래 오브젝트 삽입
            for(int i = 0; i < mParentObj.Count; ++i)
            {
                foreach (Transform child in mParentObj[i])
                {
                    if (child.TryGetComponent(out equipItem))
                    {
                        mEquipItem[child.name] = equipItem;
                    }
                }
            }
        }
    }   //초기화

    /// <summary>
    /// strItemName : 장착 할 아이템 이름
    /// </summary>
    public void Equip(string strItemName)
    {
        mEquipItem[strItemName].SetActiveStatu(true);     //장비 활성화 비활성화 설정
        //switch (mEquipItem[strItemName].GetEquipParts)
        //{
        //    case EPlayerEquipParts.RightHand:
        //    case EPlayerEquipParts.LeftHand:
        //        //플레이어의 타겟 부모 가져오기
        //        Transform targetParents = GameManager.Instance.GetPlayer.PlayerPivot.GetPivotTransform(mEquipItem[strItemName].GetEquipParts);

        //        mEquipItem[strItemName].GetPivot.SetParent(targetParents);          //부모 설정
        //        mEquipItem[strItemName].GetPivot.localPosition = Vector3.zero;      //로컬좌표 위치이동
        //        break;
        //}
    }   //아이템 이름에 해당하는 오브젝트 활성화

    /// <summary>
    /// strItemName : 장착 해제 할 아이템 이름
    /// </summary>
    public void UnEquip(string strItemName)
    {
        mEquipItem[strItemName].SetActiveStatu(false);     //장비 활성화 비활성화 설정
        //mEquipItem[strItemName].GetPivot.SetParent(mParentObj[0]);     //장비 부모위치 변경
    }   //아이템 이름에 해당하는 오브젝트 비활성화
}