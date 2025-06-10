using System.Collections.Generic;
using UnityEngine;

public class DropItem : PooledObject, IInteractAble
{
    public override event System.Action ReturnPool;

    private int miItemKey;      //������ Ű
    private int miCount;        //������ ����
    private int miRate;         //������ ���
    [SerializeField] List<ParticleSystem> mParticles;   //��ƼŬ
    private ParticleSystem.MainModule mMainModule;       //��ƼŬ main ���� ����
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
    /// pos : ���� ��ġ
    /// iItemKey : ������ Ű��
    /// iItemCount : ������ ������
    /// iRate : �Ƹ� �������� ��� ��ް�
    /// </summary>
    public void Active(Vector3 pos, int iItemKey, int iItemCount, int iRate = 0)
    {
        pos.y += 1.0f;    //����
        mTransform.position = pos;    //���� ��ġ
        miItemKey = iItemKey;    //������ Ű
        miCount = iItemCount;    //���� ����
        miRate = iRate;    //������ ���(��ȭ)
        //������ Ƽ� �°� �� ����
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
            mMainModule.startColor = mItemColor;  //���� ����
        }
        gameObject.SetActive(true);        //Ȱ��ȭ
    }   //Ȱ��ȭ

    public void Interaction()
    {
        Inventory inventory = UIManager.Instance.GetGUI(EGUI.Inventory) as Inventory;
        
        //ȹ���� ���� �Ǻ�
        int iPrevCount = miCount;
        miCount = inventory.Add(SlotValueType.Count, miItemKey, miCount);

        //���� ��ȭ�� ���� �� ������ ȹ�� UI ǥ��
        if(iPrevCount != miCount)
        {
            (UIManager.Instance.GetGUI(EGUI.Player) as PlayerPanel).
GetItemImage.ShowItem(miItemKey, iPrevCount - miCount);  //������ ȹ�� �� UIǥ��
        }

        //�������� ���� ��
        if (miCount == 0)
        {
            gameObject.SetActive(false);   //��Ȱ��ȭ
            ReturnPool?.Invoke();
        }
    }   //��ȣ�ۿ� �� �ۿ�
}
