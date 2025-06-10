using System.Collections.Generic;
using UnityEngine;

public class DropItem : PooledObject, IInteractAble
{
    public override event System.Action ReturnPool;

    private int miItemKey;      //아이템 키
    private int miCount;        //아이템 개수
    private int miRate;         //아이템 등급
    [SerializeField] List<ParticleSystem> mParticles;   //파티클
    private ParticleSystem.MainModule mMainModule;       //파티클 main 담을 변수
    private Color mItemColor;
    public override void Initialize()
    {
        EPooledObject = EPooledObject.DropItem;
        miItemKey = 0;
        miCount = 0;
        miRate = 0;
        if (mTransform == null)
        {
            mTransform = GetComponent<Transform>();
        }
        
        gameObject.SetActive(false);
    }

    public override void UnActive()
    {
        ReturnPool?.Invoke();
    }

    /// <summary>
    /// pos : 스폰 위치
    /// iItemKey : 아이템 키값
    /// iItemCount : 아이템 개수값
    /// iRate : 아머 아이템일 경우 등급값
    /// </summary>
    public void Active(Vector3 pos, int iItemKey, int iItemCount, int iRate = 0)
    {
        pos.y += 1.0f;    //높이
        mTransform.position = pos;    //시작 위치
        miItemKey = iItemKey;    //아이템 키
        miCount = iItemCount;    //개수 설정
        miRate = iRate;    //아이템 등급(강화)
        //아이템 티어에 맞게 색 변경
        for (int i = 0; i < mParticles.Count; ++i)
        {
            mMainModule = mParticles[i].main;
            switch (GameManager.Instance.GetDataMgr().GetItemData(miItemKey).Grade)
            {
                case EItemGrade.Normal:
                    mItemColor = Color.white;
                    break;
                case EItemGrade.Rare:
                    mItemColor = Color.blue;
                    break;
                case EItemGrade.Unique:
                    mItemColor = Color.red;
                    break;
                case EItemGrade.Legend:
                    mItemColor = Color.yellow;
                    break;
                default:
                    Debug.LogError("Grade is Default");
                    break;
            }
            mMainModule.startColor = mItemColor;  //색상 적용
        }
        gameObject.SetActive(true);        //활성화
    }   //활성화

    public void Interaction()
    {
        Inventory inventory = UIManager.Instance.GetGUI(EGUI.Inventory) as Inventory;
        
        //획득한 갯수 판별
        int iPrevCount = miCount;
        miCount = inventory.Add(SlotValueType.Count, miItemKey, miCount);

        //갯수 변화가 있을 때 아이템 획득 UI 표시
        if(iPrevCount != miCount)
        {
            (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).
GetItemImage.ShowItem(miItemKey, iPrevCount - miCount);  //아이템 획득 시 UI표시
        }

        //아이템을 습득 시
        if (miCount == 0)
        {
            gameObject.SetActive(false);   //비활성화
            ReturnPool?.Invoke();
        }
    }   //상호작용 시 작용
}
