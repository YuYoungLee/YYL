using System.Collections.Generic;
using UnityEngine;

//�÷��̾� �Ǻ� Ÿ��
public enum EPlayerEquipParts
{
    RightHand,      //������
    LeftHand,       //�޼�
    None,
}

public class PlayerPivot : MonoBehaviour
{
    [SerializeField] private List<Transform> mPivot;    //�Ǻ� transform ����Ʈ

    /// <summary>
    /// eEquipParts : ���� ���� Ÿ��
    /// </summary>
    public Transform GetPivotTransform(EPlayerEquipParts eEquipParts)
    {
        switch (eEquipParts)
        {
            case EPlayerEquipParts.RightHand:
            case EPlayerEquipParts.LeftHand:
                return mPivot[(int)eEquipParts];    //�Ǻ� ����
        }

        return null;
    }
}
