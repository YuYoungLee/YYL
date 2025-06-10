using System.Collections.Generic;
using UnityEngine;

public class EquipItemManager : MonoBehaviour
{
    [SerializeField] private List<Transform> mParentObj;     //������ �θ� ��ġ

    private Dictionary<string, EquipItem> mEquipItem;      //���� ������ ������Ʈ

    public void Initialize()
    {
        //������ ��ųʸ� ����
        if(mEquipItem == null)
        {
            mEquipItem = new Dictionary<string, EquipItem>();
            EquipItem equipItem;
            //�θ� �Ʒ� ������Ʈ ����
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
    }   //�ʱ�ȭ

    /// <summary>
    /// strItemName : ���� �� ������ �̸�
    /// </summary>
    public void Equip(string strItemName)
    {
        mEquipItem[strItemName].SetActiveStatu(true);     //��� Ȱ��ȭ ��Ȱ��ȭ ����
        //switch (mEquipItem[strItemName].GetEquipParts)
        //{
        //    case EPlayerEquipParts.RightHand:
        //    case EPlayerEquipParts.LeftHand:
        //        //�÷��̾��� Ÿ�� �θ� ��������
        //        Transform targetParents = GameManager.Instance.GetPlayer.PlayerPivot.GetPivotTransform(mEquipItem[strItemName].GetEquipParts);

        //        mEquipItem[strItemName].GetPivot.SetParent(targetParents);          //�θ� ����
        //        mEquipItem[strItemName].GetPivot.localPosition = Vector3.zero;      //������ǥ ��ġ�̵�
        //        break;
        //}
    }   //������ �̸��� �ش��ϴ� ������Ʈ Ȱ��ȭ

    /// <summary>
    /// strItemName : ���� ���� �� ������ �̸�
    /// </summary>
    public void UnEquip(string strItemName)
    {
        mEquipItem[strItemName].SetActiveStatu(false);     //��� Ȱ��ȭ ��Ȱ��ȭ ����
        //mEquipItem[strItemName].GetPivot.SetParent(mParentObj[0]);     //��� �θ���ġ ����
    }   //������ �̸��� �ش��ϴ� ������Ʈ ��Ȱ��ȭ
}