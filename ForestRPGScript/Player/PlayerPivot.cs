using System.Collections.Generic;
using UnityEngine;

//플레이어 피봇 타입
public enum EPlayerEquipParts
{
    RightHand,      //오른손
    LeftHand,       //왼손
    None,
}

public class PlayerPivot : MonoBehaviour
{
    [SerializeField] private List<Transform> mPivot;    //피봇 transform 리스트

    /// <summary>
    /// eEquipParts : 장착 파츠 타입
    /// </summary>
    public Transform GetPivotTransform(EPlayerEquipParts eEquipParts)
    {
        switch (eEquipParts)
        {
            case EPlayerEquipParts.RightHand:
            case EPlayerEquipParts.LeftHand:
                return mPivot[(int)eEquipParts];    //피봇 리턴
        }

        return null;
    }
}
